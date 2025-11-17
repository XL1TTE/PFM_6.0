using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Requests;
using Domain.Extentions;
using Domain.Levels.Components;
using Domain.Levels.Mono;
using Domain.Monster.Mono;
using Domain.Monster.Requests;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Mono;
using Domain.UI.Requests;
using DS.Files;
using Game;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleSceneInitializeEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattleSceneInitializeState> stash_state;

        public void OnAwake()
        {
            evt_onStateEnter = SM.m_World.GetEvent<OnStateEnterEvent>();

            stash_state = SM.m_World.GetStash<BattleSceneInitializeState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateEnter.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Enter(evt.StateEntity);
                }
            }
        }

        public void Dispose()
        {

        }

        private void Enter(Entity stateEntity)
        {
            EnterAsync(stateEntity).Forget();
        }

        private async UniTask EnterAsync(Entity stateEntity)
        {

            var config = DataStorage.GetRecordFromFile<BattleConfig, LoadConfig>();

            if (config.m_prefab_level != null)
            {

                Object.Instantiate(config.m_prefab_level); // instantiate level prefab

                await UniTask.Yield(); // Waiting for all entities creation.
                Battle.SpawnEnemiesOnLoad(World);
            }

            Battle.SpawnMonstersOnLoad(World);

            BattleUiRefs.Instance.m_NextTurnButton.Hide();

            SM.ExitState<BattleSceneInitializeState>();
            SM.EnterState<PreBattlePlanningNotificationState>();
        }

        private bool IsValid(Entity stateEntity)
        {
            if (stash_state.Has(stateEntity))
            {
                return true;
            }
            return false;
        }
    }
}

