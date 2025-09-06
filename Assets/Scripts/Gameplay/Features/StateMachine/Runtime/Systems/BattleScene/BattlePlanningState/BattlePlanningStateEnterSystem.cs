using System.Linq;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Mono;
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

        private Stash<BattlePlanningState> stash_state;

        public void OnAwake()
        {
            evt_onStateEnter = StateMachineWorld.Value.GetEvent<OnStateEnterEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattlePlanningState>();
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
        
        private void Enter(Entity stateEntity){
            
            /* ########################################## */
            /*              Change plate text             */
            /* ########################################## */
            
            BattleFieldUIRefs.Instance.InformationBoardWidget.ChangeText("Preparation");

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
        }
        
        private bool IsValid(Entity stateEntity){
            if(stash_state.Has(stateEntity)){
                return true;
            }
            return false;
        }
    }
}

