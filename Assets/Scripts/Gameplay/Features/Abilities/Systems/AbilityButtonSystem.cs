using System;
using System.Linq;
using Core.Utilities;
using Domain.Abilities.Tags;
using Domain.AbilityGraph;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.Notificator;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TurnSystem.Tags;
using Domain.UI.Requests;
using Domain.UI.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

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

            ProcessTargetSelection();

            ProcessHoverEffect();

        }

        private void ProcessOwnerBusyState()
        {
            // For actors which have active ability buttons
            // check if actor is busy. Busy -> Disable button. Otherwise -> Enable button. 
            foreach (var evt in evt_ActorStatesChanged.publishedChanges)
            {
                var abilityButton = F.FindAbilityButtonByOwner(evt.m_Actor, World);
                if (abilityButton.isNullOrDisposed(World)) { return; }

                if (V.IsActorBusy(evt.m_Actor, World))
                {
                    DisableButton(abilityButton);
                }
                else
                {
                    EnableButton(abilityButton);
                }
            }
        }

        private void ProcessTargetSelection()
        {
            foreach (var button in f_targetSelectionResults)
            {
                var abiltiyOwner = stash_AbilityButtonTag.Get(button).m_ButtonOwner;

                ref var result = ref GU.GetTargetSelectionResult(button, World);

                if (result.m_Status == TargetSelectionStatus.Success)
                {
                    SetSelectedEffect(button, false);
                    UseAbility(abiltiyOwner, result);
                    result.m_IsProcessed = true;
                }
                else if (result.m_Status == TargetSelectionStatus.Failed)
                {
                    SetSelectedEffect(button, false);
                    result.m_IsProcessed = true;
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

        private void UseAbility(Entity abiltiyOwner, TargetSelectionResult result)
        {
            var targets = GU.GetCellOccupiers(result.m_SelectedCells, World);

            req_AbiltiyActivation.Publish(new ActivateAbilityRequest
            {
                m_AbilityTemplateID = "abt_PhysicalAttack",
                m_Caster = abiltiyOwner,
                m_Targets = targets.ToList()
            });
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

                SetSelectedEffect(evt.ClickedButton, true);

                var owner = stash_AbilityButtonTag.Get(evt.ClickedButton).m_ButtonOwner;

                var allOptions = GU.FindAttackOptionsCellsFor(owner, World);
                var avaibleOptions = F.FilterCellsWithEnemies(allOptions, World);

                req_TargetSelection.Publish(new TargetSelectionRequest
                {
                    m_Sender = evt.ClickedButton,
                    m_TargetsAmount = 1,
                    m_AwaibleOptions = avaibleOptions.ToList(),
                    m_UnavaibleOptions = allOptions.Except(avaibleOptions).ToList()

                });
            }


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


