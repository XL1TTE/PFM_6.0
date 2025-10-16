using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.Abilities.Mono;
using Domain.Abilities.Tags;
using Domain.AbilityGraph;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.CursorDetection.Components;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.Notificator;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TargetSelection.Tags;
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
        private FilterBuilder f_turnTaker;
        private Filter f_targetSelectionResults;
        private Request<TargetSelectionRequest> req_TargetSelection;
        private Event<ButtonClickedEvent> evt_ButtonClicked;

        private Event<ActorActionStatesChanged> evt_ActorStatesChanged;
        private Stash<AbiltiyButtonTag> stash_AbilityButtonTag;
        private Stash<ButtonTag> stash_ButtonTag;

        public World World { get; set; }

        public void OnAwake()
        {
            f_turnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>();

            f_targetSelectionResults = World.Filter
                .With<AbiltiyButtonTag>()
                .With<TargetSelectionResult>()
                .Build();

            req_TargetSelection = World.GetRequest<TargetSelectionRequest>();

            evt_ButtonClicked = World.GetEvent<ButtonClickedEvent>();

            evt_ActorStatesChanged = World.GetEvent<ActorActionStatesChanged>();

            stash_AbilityButtonTag = World.GetStash<AbiltiyButtonTag>();
            stash_ButtonTag = World.GetStash<ButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_ButtonClicked.publishedChanges)
            {
                if (!IsAbilityButtonClicked(evt)) { return; } // Ignore if not abiltiy button was clicked.

                ProcessClick(evt.ClickedButton);
            }

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


            foreach (var r in f_targetSelectionResults)
            {
                ref var result = ref GU.GetTargetSelectionResult(r, World);
                result.m_IsProcessed = true;

                Debug.Log($"Target -> Entity: {result.m_SelectedTargets[0]}");
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

        private void ProcessClick(Entity abiltiyButton)
        {
            var owner = stash_AbilityButtonTag.Get(abiltiyButton).m_ButtonOwner;

            var allOptions = GU.FindAttackOptionsCellsFor(owner, World);
            var avaibleOptions = F.FilterCellsWithEnemies(allOptions, World);

            req_TargetSelection.Publish(new TargetSelectionRequest
            {
                m_Sender = abiltiyButton,
                m_TargetsAmount = 1,
                m_AwaibleOptions = avaibleOptions.ToList(),
                m_UnavaibleOptions = allOptions.Except(avaibleOptions).ToList()

            });
        }

        private bool IsAbilityButtonClicked(ButtonClickedEvent @event)
        => stash_AbilityButtonTag.Has(@event.ClickedButton) ? true : false;

        public void Dispose()
        {

        }

    }
}


