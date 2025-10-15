using System;
using System.Collections.Generic;
using Domain.BattleField.Requests;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.TargetSelection.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TargetSelectionCleanupSystem : ICleanupSystem
    {
        public World World { get; set; }

        /// <summary>
        /// Sessions which marked as completed.
        /// </summary>
        private Filter f_ActiveSessionsToComplete;
        private Filter f_selOpt;

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
            f_ActiveSessionsToComplete = World.Filter
                .With<TargetSelectionSession>()
                .With<TargetSelectionResult>()
                .Build();

            f_selOpt = World.Filter.With<SelectionOptionTag>().Build();

            req_SelectCellsView = World.GetRequest<ChangeCellViewToSelectedRequest>();

            stash_selectionResult = World.GetStash<TargetSelectionResult>();
            stash_SelectionOptionTag = World.GetStash<SelectionOptionTag>();
            stash_TargetSelectionSession = World.GetStash<TargetSelectionSession>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var session in f_ActiveSessionsToComplete)
            {
                if (IsExpireTime(session, deltaTime))
                {
                    CleanupResult(session);
                }
            }
        }

        private bool IsExpireTime(Entity session, float deltaTime)
        {
            ref var result = ref stash_selectionResult.Get(session);

            result.m_ExpireIn -= deltaTime;

            if (result.m_ExpireIn <= 0.0f)
            {
                return true;
            }

            if (result.m_IsProcessed)
            {
                return true;
            }

            return false;
        }

        private void CleanupResult(Entity sessionOwner)
        {
            stash_TargetSelectionSession.Remove(sessionOwner);
            stash_selectionResult.Remove(sessionOwner);
        }

        public void Dispose()
        {

        }
        private void CleanupSelectionOptionsTags()
        {
            foreach (var selOpt in f_selOpt) // Remove all selection option marks.
            {
                stash_SelectionOptionTag.Remove(selOpt);
            }
        }
    }
}
