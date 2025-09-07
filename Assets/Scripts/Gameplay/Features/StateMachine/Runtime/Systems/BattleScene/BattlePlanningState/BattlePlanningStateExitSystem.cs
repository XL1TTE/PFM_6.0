using System.Linq;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateExitSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateExitEvent> evt_onStateExit;

        private Stash<BattlePlanningState> stash_state;

        public void OnAwake()
        {
            evt_onStateExit = StateMachineWorld.Value.GetEvent<OnStateExitEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattlePlanningState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_onStateExit.publishedChanges){
                if(IsValid(evt.StateEntity)){
                    Exit(evt.StateEntity);
                }
            }
        }

        public void Dispose()
        {

        }
        
        private void Exit(Entity stateEntity){
            
            Filter spawnCellsFilter = World.Filter.With<TagMonsterSpawnCell>().Build();

            var highlightMonsterSpawnCellsReq = World.Default.GetRequest<ChangeCellViewToSelectRequest>();

            highlightMonsterSpawnCellsReq.Publish(
                    new ChangeCellViewToSelectRequest
                    {
                        Cells = spawnCellsFilter.AsEnumerable(), 
                        State = ChangeCellViewToSelectRequest.SelectState.Disabled}
                    , true);

            var req_markMonsterSpawnCellsAsDropTargets =
                World.Default.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest {
                state = MarkMonsterSpawnCellsAsDropTargetRequest.State.Disable }, true);

            var exitPlngStateBtn = World.Filter.With<ExitPlanningStageButtonTag>().Build().FirstOrDefault();
            if (exitPlngStateBtn.IsExist())
            {
                if (World.TryGetComponent<ExitPlanningStageButtonTag>(exitPlngStateBtn, out var exitPlngStateBtnView))
                {
                    exitPlngStateBtnView.View.DestroySelf();
                }
            }
        }
        
        private bool IsValid(Entity stateEntity){
            if (stash_state.Has(stateEntity)){
                return true;
            }
            return false;
        }
    }
}

