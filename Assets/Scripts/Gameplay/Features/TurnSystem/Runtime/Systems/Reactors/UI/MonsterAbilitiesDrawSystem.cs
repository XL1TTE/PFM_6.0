using System.Collections.Generic;
using Domain.Abilities.Components;
using Domain.Abilities.Providers;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Domain.UI.Mono;
using Persistence.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.TurnSystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterAbilitiesDrawSystem : ISystem
    {
        public World World { get; set; }

        private Filter filter_monsterTurnTaker;

        private Event<NextTurnStartedEvent> evt_nextTurnStarted;
        private Stash<AbilitiesComponent> stash_Abilities;
        private const string _moveAbilityRecordID = "abt_moveAbility";
        private readonly GameObject m_AbilityButtonPrefab = GameResources.p_AbilityButton;


        private List<GameObject> _abilityBtnsCache = new();

        public void OnAwake()
        {
            filter_monsterTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();

            stash_Abilities = World.GetStash<AbilitiesComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_nextTurnStarted.publishedChanges)
            {
                if (filter_monsterTurnTaker.IsEmpty() == false)
                { // means that now is monster turn

                    var abilityOwner = filter_monsterTurnTaker.First();

                    ClearAbilityViews();
                    DrawMoveAbility(abilityOwner);
                    DrawAbilities(abilityOwner);
                }
                else
                {
                    ClearAbilityViews();
                }
            }
        }

        public void Dispose()
        {

        }

        private void DrawMoveAbility(Entity abilityOwner)
        {
            if (DataBase.TryFindRecordByID(_moveAbilityRecordID, out var record))
            {
                var slot = BattleFieldUIRefs.Instance.BookWidget.m_MoveButtonSlot;
                if (DataBase.TryGetRecord<PrefabComponent>(record, out var prefab))
                {
                    GameObject ability_btn = Object.Instantiate(prefab.Value, slot);
                    if (ability_btn.TryFindComponent<AbilityButtonTagProvider>(out var ability))
                    {
                        ability.GetData().m_ButtonOwner = abilityOwner;
                    }
                    _abilityBtnsCache.Add(ability_btn);
                }
            }
        }
        private void DrawAbilities(Entity abilityOwner)
        {
            if (stash_Abilities.Has(abilityOwner) == false) { return; }
            var t_abilities = stash_Abilities.Get(abilityOwner);

            if (t_abilities.m_LeftHandAbility?.m_Value != null)
            {
                var slot = BattleFieldUIRefs.Instance.BookWidget.m_FirstHandAbilitySlot;
                GameObject ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_LeftHandAbility);
                _abilityBtnsCache.Add(ability_btn);
            }
            if (t_abilities.m_RightHandAbility?.m_Value != null)
            {
                var slot = BattleFieldUIRefs.Instance.BookWidget.m_SecondHandAbilitySlot;
                GameObject ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_RightHandAbility);
                _abilityBtnsCache.Add(ability_btn);
            }
            if (t_abilities.m_HeadAbility?.m_Value != null)
            {
                var slot = BattleFieldUIRefs.Instance.BookWidget.m_HeadAbilitySlot;
                GameObject ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_HeadAbility);
                _abilityBtnsCache.Add(ability_btn);
            }
        }

        private void AttachAbilityOwnerToView(GameObject view, Entity owner)
        {
            if (view.TryFindComponent<AbilityButtonTagProvider>(out var ability))
            {
                ability.GetData().m_ButtonOwner = owner;
            }
        }
        private void AttachAbilityToView(GameObject view, AbilityData ability)
        {
            if (view.TryFindComponent<AbilityButtonTagProvider>(out var abilityTag))
            {
                abilityTag.GetData().m_Ability = ability;
            }
        }

        private void ClearAbilityViews()
        {
            foreach (var item in _abilityBtnsCache)
            {
                Object.Destroy(item);
            }
            _abilityBtnsCache.Clear();
        }
    }
}


