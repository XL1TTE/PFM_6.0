using System.Collections;
using Core.Utilities;
using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Gameplay.Features.DragAndDrop.Components;
using Scellecs.Morpeh;
using UI.Requests;
using UI.Widgets;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
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

            var genMonsterReq = World.GetRequest<GenerateMonstersRequest>();

            genMonsterReq.Publish(new GenerateMonstersRequest
            {
                MosntersCount = 1
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

