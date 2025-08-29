using System.Collections;
using System.Collections.Generic;
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
using Domain.UI.Requests;
using Domain.UI.Widgets;
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
    public sealed class BattlePlanningStateInitializeEnterSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter f_state;

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattlePlanningInitializeState> stash_state;
        private Stash<DropTargetComponent> stash_dropTarget;
        
        private Request<ChangeMonsterDraggableStateRequest> req_monsterDrag;


        public void OnAwake()
        {
            evt_onStateEnter = StateMachineWorld.Value.GetEvent<OnStateEnterEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattlePlanningInitializeState>();
            stash_dropTarget = World.GetStash<DropTargetComponent>();

            req_monsterDrag = World.GetRequest<ChangeMonsterDraggableStateRequest>();
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

                    if (DataBase.TryGetRecord<EnemiesPool>(lvl_record, out var ep))
                    {
                        var enemiesToSpawn = ep.Value;
                    }
                }
            }
            
            yield return new WaitForEndOfFrame();
            
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

            World.GetRequest<FullScreenNotificationRequest>().Publish(
            new FullScreenNotificationRequest{
                state = FullScreenNotificationRequest.State.Enable,
                Message = "Planning stage",
                Tip = "Press LMB to continue..."
            }, true);
                            
            yield return new WaitForMouseDown(0);
            
            /* ########################################## */
            /*              Change plate text             */
            /* ########################################## */
            
            PlateWithText.Instance.Show("Planning stage");
            

            World.GetRequest<FullScreenNotificationRequest>().Publish(
            new FullScreenNotificationRequest
            {
                state = FullScreenNotificationRequest.State.Disable,
            }, true);

            /* ############################################## */
            /*         Hightlight monster spawn cells         */
            /* ############################################## */

            Filter spawnCellsFilter = World.Filter.With<TagMonsterSpawnCell>().Build();

            var highlightMonsterSpawnCellsReq = World.Default.GetRequest<ChangeCellViewToSelectRequest>();

            highlightMonsterSpawnCellsReq.Publish(
                    new ChangeCellViewToSelectRequest
                    {
                        Cells = spawnCellsFilter.AsEnumerable(), 
                        State = ChangeCellViewToSelectRequest.SelectState.Enabled}
                    , true);

            var req_markMonsterSpawnCellsAsDropTargets =
                World.Default.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest { DropRadius = 30.0f, 
                state = MarkMonsterSpawnCellsAsDropTargetRequest.State.Enable }, true);

            StateMachineWorld.ExitState<BattlePlanningInitializeState>();
            StateMachineWorld.EnterState<BattlePlanningState>();
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

