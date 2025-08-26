using System.Collections;
using System.Collections.Generic;
using Core.Utilities;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Requests;
using Domain.Extentions;
using Domain.Monster.Mono;
using Domain.Monster.Requests;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.UI.Requests;
using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattlePlanningState> stash_battlePlanning;
        private Stash<DropTargetComponent> stash_dropTarget;
        
        private Request<ChangeMonsterDraggableStateRequest> req_monsterDrag;


        public void OnAwake()
        {
            evt_onStateEnter = World.GetEvent<OnStateEnterEvent>();

            stash_battlePlanning = World.GetStash<BattlePlanningState>();
            stash_dropTarget = World.GetStash<DropTargetComponent>();

            req_monsterDrag = World.GetRequest<ChangeMonsterDraggableStateRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_onStateEnter.publishedChanges){
                if(isStateValid(evt.StateEntity)){
                    Enter(evt.StateEntity);
                }
            }
        }

        public void Dispose()
        {

        }
        
        
        private IEnumerator EnterRoutine(Entity stateEntity){

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
                    new MosnterData(
                        "mp_DammyHead",
                        "mp_DammyArm",
                        "mp_DammyArm",
                        "mp_DammyTorso",
                        "mp_DinLeg",
                        "mp_DinLeg"
                    )
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
                new MarkMonsterSpawnCellsAsDropTargetRequest { DropRadius = 1.0f, 
                state = MarkMonsterSpawnCellsAsDropTargetRequest.State.Enable }, true);
        }
        
        private void Enter(Entity stateEntity){
            RellayCoroutiner.Run(EnterRoutine(stateEntity));
        }
        
        private bool isStateValid(Entity stateEntity){
            if(!stash_battlePlanning.Has(stateEntity)){return false;}
            else{return true;}
        }
    }
}

