using System.Collections.Generic;
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

        private const string _moveAbilityRecordID = "abt_moveAbility";
        private const string _attackAbilityRecordID = "abt_attackAbility";


        private List<GameObject> _abilityBtnsCache = new();

        public void OnAwake()
        {
            filter_monsterTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();
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
                    DrawAttackAbility(abilityOwner);
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
                var slot = BattleFieldUIRefs.Instance.BookWidget.MoveButtonSlot;
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
        private void DrawAttackAbility(Entity abilityOwner)
        {
            if (DataBase.TryFindRecordByID(_attackAbilityRecordID, out var record))
            {
                var slot = BattleFieldUIRefs.Instance.BookWidget.AttackButtonSlot;
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


