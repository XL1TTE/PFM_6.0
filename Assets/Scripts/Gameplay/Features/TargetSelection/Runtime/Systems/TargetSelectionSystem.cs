using System;
using System.Collections.Generic;
using Domain.BattleField.Requests;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TargetSelection.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.TargetSelection.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TargetSelectionSystem : ISystem
    {
        public World World { get; set; }

        /// <summary>
        /// Filter of entities with special tag, which let system know that this entity
        /// can be selected by player.
        /// </summary>
        private Filter f_SelectOptionsUnderCursor;
        private Filter f_CompletedSessions;
        private Filter f_selOpt;

        /// <summary>
        /// All currently active target selections. 
        /// Should always be one active selection at the moment.
        /// </summary>
        private Filter f_ActiveSelections;

        /// <summary>
        /// Request for target selection. 
        /// Can be send by any system when player should pick the targets.
        /// </summary>
        private Request<TargetSelectionRequest> req_TargetSelection;

        /// <summary>
        /// Request to cells render system for change cell view to selected. 
        /// </summary>
        private Request<ChangeCellViewToSelectedRequest> req_SelectCellsView;

        /// <summary>
        /// Event that will be send when target selection completed. 
        /// </summary>
        private Stash<TargetSelectionResult> stash_selectionResult;

        /// <summary>
        /// Stash with selection option tags, which system will use to mark targets. 
        /// </summary>
        private Stash<SelectionOptionTag> stash_SelectionOptionTag;
        /// <summary>
        /// Target selection process state. Will be added to request sender entity.
        /// </summary>
        private Stash<TargetSelectionSession> stash_TargetSelectionSession;

        public void OnAwake()
        {
            f_SelectOptionsUnderCursor = World.Filter
                .With<SelectionOptionTag>()
                .With<UnderCursorComponent>()
                .Build();

            f_CompletedSessions = World.Filter
                .With<TargetSelectionSession>()
                .With<TargetSelectionResult>()
                .Build();

            f_selOpt = World.Filter.With<SelectionOptionTag>().Build();

            f_ActiveSelections = World.Filter
                .With<TargetSelectionSession>()
                .Without<TargetSelectionResult>()
                .Build();

            req_TargetSelection = World.GetRequest<TargetSelectionRequest>();
            req_SelectCellsView = World.GetRequest<ChangeCellViewToSelectedRequest>();

            stash_selectionResult = World.GetStash<TargetSelectionResult>();
            stash_SelectionOptionTag = World.GetStash<SelectionOptionTag>();
            stash_TargetSelectionSession = World.GetStash<TargetSelectionSession>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_TargetSelection.Consume()) // Initiating target selections
            {
                foreach (var owner in f_ActiveSelections)
                {
                    if (owner.Id == req.m_Sender.Id) // If sender the same as it was, just ignore.
                    {
                        //CompleteSelection(owner, TargetSelectionStatus.Failed);
                        return;
                    }
                    CompleteSelection(owner, TargetSelectionStatus.Failed);
                }

                foreach (var owner in f_CompletedSessions)
                {
                    if (owner.Id == req.m_Sender.Id) // If sender try to send request again but not handled previous result -> exception.
                    {
                        throw new Exception($"You are trying to send target selection request from Entity:{owner.Id} multiple times, without handling previous results. Filter for Target Selection Result component on your sender and set m_Processed flag to true.");
                    }
                }

                OpenTargetSelectionSession(req.m_Sender, req);
                EnterTargetSelectionState();
                return; // Only one request should be processed in one frame. Delay processing until next frame.
            }

            foreach (var session in f_ActiveSelections)
            {
                ProcessSelection(session);
            }
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Open new target selection session by attaching state component to request sender.
        /// </summary>
        /// <param name="owner">Request sender entity.</param>
        /// <param name="req">Request data.</param>
        private void OpenTargetSelectionSession(Entity owner, TargetSelectionRequest req)
        {
            if (owner.isNullOrDisposed(World))
            {
                throw new Exception("You trying to request target selection, but request sender was null.");
            }

            stash_TargetSelectionSession.Set(owner, new TargetSelectionSession
            {
                m_TargetsAmount = req.m_TargetsAmount,
                m_AwaibleOptions = req.m_AwaibleOptions,
                m_UnavaibleOptions = req.m_UnavaibleOptions,
                m_SelectedOptions = new List<Entity>()
            });
            HandleAwaibleTargets(req.m_AwaibleOptions);
            HandleUnawaibleTargets(req.m_UnavaibleOptions);
        }

        /// <summary>
        /// Removes selection option tag from all entities.
        /// </summary>
        private void CleanupSelectionOptionsTags()
        {
            foreach (var selOpt in f_selOpt) // Remove all selection option marks.
            {
                stash_SelectionOptionTag.Remove(selOpt);
            }
        }

        /// <summary>
        /// Complete selection with cell states recovery.
        /// </summary>
        /// <param name="status">Completion status code.</param>
        /// <param name="selectedTargets">Selected targets. Empty by default.</param>
        private void CompleteSelection(Entity sessionOwner, TargetSelectionStatus status)
        {
            var session = stash_TargetSelectionSession.Get(sessionOwner);
            SendTargetSelectionResult(sessionOwner, status);
            CleanupSelectionOptionsTags();
            RecoverTargetsViews(session);
            ExitTargetSelectionState();
        }

        /// <summary>
        /// Sends target selection completed event.
        /// </summary>
        /// <param name="status">Completion status code.</param>
        /// <param name="selectedTargets">Selected targets. Empty by default.</param>
        private void SendTargetSelectionResult(Entity sessionOwner, TargetSelectionStatus status)
        {
            var session = stash_TargetSelectionSession.Get(sessionOwner);
            stash_selectionResult.Set(sessionOwner, new TargetSelectionResult
            {
                m_Status = status,
                m_SelectedCells = session.m_SelectedOptions,
                m_IsProcessed = false,
                m_ExpireIn = 1
            });
        }

        /// <summary>
        /// Process target selection by handling player input.
        /// </summary>
        private void ProcessSelection(Entity sessionOwner)
        {
            if (sessionOwner.isNullOrDisposed(World)) // if session is not valid any more.
            {
                CompleteSelection(sessionOwner, TargetSelectionStatus.Failed);
                return;
            }

            ref var session = ref stash_TargetSelectionSession.Get(sessionOwner);

            if (Input.GetMouseButtonDown(0) && !f_SelectOptionsUnderCursor.IsEmpty()) // if player try to select target.
            {
                SelectTarget(f_SelectOptionsUnderCursor.FirstOrDefault(), ref session);
                if (IsSelectionCompleted(session))
                {
                    CompleteSelection(sessionOwner, TargetSelectionStatus.Success);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                CompleteSelection(sessionOwner, TargetSelectionStatus.Failed);
                return;
            }

            // if target selection state was force removed, we shoud exit target selection.
            if (SM.IsStateActive<TargetSelectionState>(out var state) == false)
            {
                CompleteSelection(sessionOwner, TargetSelectionStatus.Failed);
                return;
            }
        }

        private void SelectTarget(Entity target, ref TargetSelectionSession session)
        {
            if (target.IsExist() == false) { return; }

            ref var TargetState = ref stash_SelectionOptionTag.Get(target);

            if (TargetState.m_IsSelected) // if try to select already selected unit -> remove from selected.
            {
                session.m_SelectedOptions.Remove(target);
                TargetState.m_IsSelected = false;
            }
            else
            {
                session.m_SelectedOptions.Add(target);
                TargetState.m_IsSelected = true;
            }
        }


        /// <summary>
        /// Evaluates if target amount condition is met.
        /// </summary>
        /// <returns></returns>
        private bool IsSelectionCompleted(TargetSelectionSession session)
        {
            if (session.m_SelectedOptions.Count == Math.Min(session.m_TargetsAmount, session.m_AwaibleOptions.Count))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Adds target selection state to game state machine.
        /// </summary>
        private void EnterTargetSelectionState()
        {
            SM.EnterState<TargetSelectionState>();
        }
        /// <summary>
        /// Removes target selection state from game state machine.
        /// </summary>
        private void ExitTargetSelectionState()
        {
            SM.ExitState<TargetSelectionState>();
        }
        /// <summary>
        /// Request unawaible target highlight. 
        /// </summary>
        /// <param name="targets">Unawaible to selection targets.</param>
        private void HandleUnawaibleTargets(List<Entity> targets)
        {
            req_SelectCellsView.Publish(new ChangeCellViewToSelectedRequest
            {
                Cells = new List<Entity>(targets),
                State = ChangeCellViewToSelectedRequest.SelectState.Enabled
            });
        }

        /// <summary>
        /// Request awaible target highlight. 
        /// Mark awaible target entities with special component.
        /// </summary>
        /// <param name="targets">Awaible to selection targets.</param>
        private void HandleAwaibleTargets(List<Entity> targets)
        {
            req_SelectCellsView.Publish(new ChangeCellViewToSelectedRequest
            {
                Cells = new List<Entity>(targets),
                State = ChangeCellViewToSelectedRequest.SelectState.Enabled
            });

            foreach (var e in targets)// Mark all awaible targets.
            {
                stash_SelectionOptionTag.Add(e);
            }
        }

        /// <summary>
        /// Sends requests for cells view system to restore cell view state.
        /// </summary>
        /// <param name="session"></param>
        private void RecoverTargetsViews(TargetSelectionSession session)
        {
            req_SelectCellsView.Publish(new ChangeCellViewToSelectedRequest
            {
                Cells = new List<Entity>(session.m_AwaibleOptions),
                State = ChangeCellViewToSelectedRequest.SelectState.Disabled,
            });
            req_SelectCellsView.Publish(new ChangeCellViewToSelectedRequest
            {
                Cells = new List<Entity>(session.m_UnavaibleOptions),
                State = ChangeCellViewToSelectedRequest.SelectState.Disabled,
            });
        }

    }
}
