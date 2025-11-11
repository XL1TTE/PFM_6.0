using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Abilities.Tags;
using Domain.AbilityGraph;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Domain.Notificator;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.UI.Requests;
using Domain.UI.Tags;
using Gameplay.TargetSelection;
using Interactions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Abilities.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityButtonSystem : ISystem
    {
        private Filter f_targetSelectionResults;
        private Request<TargetSelectionRequest> req_TargetSelection;
        private Request<ActivateAbilityRequest> req_AbiltiyActivation;
        private Event<ButtonClickedEvent> evt_ButtonClicked;

        private Event<ActorActionStatesChanged> evt_ActorStatesChanged;
        private Event<OnCursorEnterEvent> evt_OnCursorEnter;
        private Event<OnCursorExitEvent> evt_OnCursorExit;
        private Stash<AbiltiyButtonTag> stash_AbilityButtonTag;
        private Stash<ButtonTag> stash_ButtonTag;

        public World World { get; set; }

        public void OnAwake()
        {
            f_targetSelectionResults = World.Filter
                .With<AbiltiyButtonTag>()
                .With<TargetSelectionResult>()
                .Build();

            req_TargetSelection = World.GetRequest<TargetSelectionRequest>();

            req_AbiltiyActivation = World.GetRequest<ActivateAbilityRequest>();

            evt_ButtonClicked = World.GetEvent<ButtonClickedEvent>();

            evt_ActorStatesChanged = World.GetEvent<ActorActionStatesChanged>();

            evt_OnCursorEnter = World.GetEvent<OnCursorEnterEvent>();
            evt_OnCursorExit = World.GetEvent<OnCursorExitEvent>();

            stash_AbilityButtonTag = World.GetStash<AbiltiyButtonTag>();
            stash_ButtonTag = World.GetStash<ButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            ProcessClick();

            ProcessOwnerBusyState();

            ProcessHoverEffect();

        }

        private void ProcessOwnerBusyState()
        {
            // For actors which have active ability buttons
            // check if actor is busy. Busy -> Disable button. Otherwise -> Enable button. 
            foreach (var evt in evt_ActorStatesChanged.publishedChanges)
            {
                var abilityButtons = F.FindAbilityButtonsByOwner(evt.m_Actor, World);

                foreach (var btn in abilityButtons)
                {
                    if (V.IsActorBusy(evt.m_Actor, World))
                    {
                        DisableButton(btn);
                    }
                    else
                    {
                        EnableButton(btn);
                    }

                }
            }
        }

        private void ProcessHoverEffect()
        {
            foreach (var evt in evt_OnCursorEnter.publishedChanges)
            {
                if (IsAbilityButton(evt.m_Entity))
                {
                    SetHoverEffect(evt.m_Entity, true);
                }
            }
            foreach (var evt in evt_OnCursorExit.publishedChanges)
            {
                if (IsAbilityButton(evt.m_Entity))
                {
                    SetHoverEffect(evt.m_Entity, false);
                }
            }
        }

        private void SetHoverEffect(Entity abilityButton, bool isActive)
        {
            ref var view = ref stash_AbilityButtonTag.Get(abilityButton).m_View;
            if (isActive)
            {
                view.EnableHoverView();
            }
            else
            {
                view.DisableHoverView();
            }
        }

        private void SetSelectedEffect(Entity abilityButton, bool isActive)
        {
            if (abilityButton.isNullOrDisposed(World)) { return; }

            ref var view = ref stash_AbilityButtonTag.Get(abilityButton).m_View;
            if (isActive)
            {
                view.EnableSelectedView();
            }
            else
            {
                view.DisableSelectiedView();
            }
        }


        private void EnableButton(Entity abilityButton)
        {
            stash_ButtonTag.Get(abilityButton).state = ButtonTag.State.Enabled;
            stash_AbilityButtonTag.Get(abilityButton).m_View.DisableUnavaibleView();
        }

        private void DisableButton(Entity abilityButton)
        {
            stash_ButtonTag.Get(abilityButton).state = ButtonTag.State.Disabled;
            stash_AbilityButtonTag.Get(abilityButton).m_View.EnableUnavaibleView();
        }


        /// <summary>
        /// Sends target selection request
        /// </summary>
        /// <param name="abiltiyButton"></param>
        private void ProcessClick()
        {
            foreach (var evt in evt_ButtonClicked.publishedChanges)
            {
                if (!IsAbilityButtonClicked(evt)) { return; } // Ignore if not abiltiy button was clicked.

                var owner = stash_AbilityButtonTag.Get(evt.ClickedButton).m_ButtonOwner;

                var ownerPos = GU.GetEntityPositionOnCell(owner, World);

                var abilityData = stash_AbilityButtonTag.Get(evt.ClickedButton).m_Ability;

                var t_shifts = GU.TransformShiftsFromSubjectLook(owner, abilityData.m_Shifts, World);
                var t_options = GU.GetCellsFromShifts(ownerPos, t_shifts, World);

                ExecuteAbilityAsync(abilityData.m_Value, evt.ClickedButton, owner, t_options, abilityData.m_TargetType, 1).Forget(); // Run execution in async
            }
        }

        private async UniTask ExecuteAbilityAsync(
            Ability ability,
            Entity abilityView,
            Entity caster,
            IEnumerable<Entity> a_cellOptions,
            TargetSelectionTypes a_type,
            uint a_maxTargets
        )
        {
            SetSelectedEffect(abilityView, true);


            Result t_result = new Result();
            foreach (var i in Interactor.GetAll<IOnTargetSelection>())
            {
                await i.Execute(a_cellOptions, a_type, a_maxTargets, World, t_result);
            }

            if (t_result.m_Value.Count() != 0)
            {
                Entity target = default;

                foreach (var target_cell in t_result.m_Value)
                {
                    switch (a_type)
                    {
                        case TargetSelectionTypes.CELL_WITH_ENEMY:
                            target = GU.GetCellOccupier(target_cell, World);
                            await ability.Execute(caster, target, World);
                            break;
                        case TargetSelectionTypes.CELL_WITH_ALLY:
                            target = GU.GetCellOccupier(target_cell, World);
                            await ability.Execute(caster, target, World);
                            break;
                        case TargetSelectionTypes.CELL_EMPTY:
                            target = target_cell;
                            await ability.Execute(caster, target, World);
                            break;
                    }
                }
            }


            if (a_type == TargetSelectionTypes.NONE)
            {
                await ability.Execute(caster, default, World);
            }

            SetSelectedEffect(abilityView, false);
        }

        private bool IsAbilityButtonClicked(ButtonClickedEvent @event)
        => stash_AbilityButtonTag.Has(@event.ClickedButton) ? true : false;

        private bool IsAbilityButton(Entity entity)
        {
            if (entity.isNullOrDisposed(World)) { return false; }
            if (stash_AbilityButtonTag.Has(entity) == false) { return false; }

            return true;
        }

        public void Dispose()
        {

        }

    }
}


