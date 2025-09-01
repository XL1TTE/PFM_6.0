
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Requests;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Tags;
using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningInitializeExitSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter f_state;

        private Event<OnStateExitEvent> evt_onStateExit;
        
        private Stash<BattlePlanningInitializeState> stash_state;

        public void OnAwake()
        {
            f_state = StateMachineWorld.Value.Filter.With<BattlePlanningInitializeState>().Build();

            evt_onStateExit = StateMachineWorld.Value.GetEvent<OnStateExitEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattlePlanningInitializeState>();

        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateExit.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Exit(evt.StateEntity);
                }
            }
        }


        private void Exit(Entity stateEntity){
            ExitRoutine(stateEntity);
        }

        private void ExitRoutine(Entity stateEntity)
        {

            PlateWithText.Instance.Hide();

            /* ############################################## */
            /*         Hightlight monster spawn cells         */
            /* ############################################## */

            Filter spawnCellsFilter = World.Filter.With<TagMonsterSpawnCell>().Build();

            var highlightMonsterSpawnCellsReq = World.Default.GetRequest<ChangeCellViewToSelectRequest>();

            highlightMonsterSpawnCellsReq.Publish(
                    new ChangeCellViewToSelectRequest
                    {
                        Cells = spawnCellsFilter.AsEnumerable(),
                        State = ChangeCellViewToSelectRequest.SelectState.Disabled
                    }, true);

            var req_markMonsterSpawnCellsAsDropTargets =
                World.Default.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest { DropRadius = 1.0f, 
                state = MarkMonsterSpawnCellsAsDropTargetRequest.State.Disable}, true);
                
            // Remove start battle button
            var exitPlngStateBtn = World.Filter.With<ExitPlanningStageButtonTag>().Build().FirstOrDefault();
            if (exitPlngStateBtn.IsExist()){
                if(World.TryGetComponent<ExitPlanningStageButtonTag>(exitPlngStateBtn, out var exitPlngStateBtnView)){
                    exitPlngStateBtnView.View.DestroySelf();
                }
            }
        }


        public void Dispose()
        {
        }

        private bool IsValid(Entity entityState)
        {
            if (stash_state.Has(entityState))
            {
                return false;
            }
            return true;
        }
    }

}
