using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
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
using Persistence.Components;
using Persistence.DB;
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
            evt_onStateEnter = StateMachineWorld.Value.GetEvent<OnStateEnterEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattleSceneInitializeState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_onStateEnter.publishedChanges){
                if(IsValid(evt.StateEntity)){
                    Enter(evt.StateEntity);
                }
            }
        }

        public void Dispose()
        {

        }
        
        private IEnumerator EnterRoutine(Entity stateEntity){
            
            /* ########################################## */
            /*                 Load level                 */
            /* ########################################## */
            if(LevelConfig.StartLevelID != null){
                if (DataBase.TryFindRecordByID(LevelConfig.StartLevelID, out var lvl_record))
                {
                    if (DataBase.TryGetRecord<PrefabComponent>(lvl_record, out var lvl_prefab))
                    {
                        if (lvl_prefab.Value == null)
                        {
                            throw new System.Exception($"Level prefab: {LevelConfig.StartLevelID} was not found.");
                        }
                        Object.Instantiate(lvl_prefab.Value); // instantiate level prefab
                    }
                    
                    yield return new WaitForEndOfFrame(); // Wait while prefab instantiated;

                    if (DataBase.TryGetRecord<EnemiesPool>(lvl_record, out var ep))
                    {
                        var enemiesToSpawn = ep.Value;
                        var spawnEnemiesReq = World.GetRequest<EnemySpawnRequest>();
                        spawnEnemiesReq.Publish(new EnemySpawnRequest{
                            EnemiesIDs = enemiesToSpawn.ToList()
                        }, true);
                    }
                }
            }
            
            /* ############################################## */
            /*           Pre-spawn monsters request           */
            /* ############################################## */

            var genMonsterReq = World.GetRequest<SpawnMonstersRequest>();

            genMonsterReq.Publish(new SpawnMonstersRequest
            {
                Monsters = new List<MosnterData>{
                    new MosnterData(
                        "mp_DinHead",
                        "mp_DinArm",
                        "mp_DinArm",
                        "mp_DinTorso",
                        "mp_DinLeg",
                        "mp_DinLeg"),
                }
            }, true);
            
            StateMachineWorld.ExitState<BattleSceneInitializeState>();
            StateMachineWorld.EnterState<PreBattlePlanningNotificationState>();
        }

        private void Enter(Entity stateEntity){
            RellayCoroutiner.Run(EnterRoutine(stateEntity));
        }
        
        private bool IsValid(Entity stateEntity){
            if(stash_state.Has(stateEntity)){
                return true;
            }
            return false;
        }
    }
}

