using System.Collections;
using Codice.CM.Client.Differences;
using Core.Utilities;
using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.Monster.Components;
using Scellecs.Morpeh;
using UI.Components;
using UI.Widgets;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateExitSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _monsters;

        private Event<OnStateExitEvent> evt_onStateExit;

        private Stash<BattlePlanningState> stash_battlePlanningState;
        private Stash<DropTargetComponent> stash_dropTarget;

        private Request<ChangeMonsterDraggableStateRequest> req_monsterDrag;


        public void OnAwake()
        {
            _monsters = World.Filter
                .With<TagMonster>()
                .Build();

            evt_onStateExit = World.GetEvent<OnStateExitEvent>();

            stash_battlePlanningState = World.GetStash<BattlePlanningState>();
            stash_dropTarget = World.GetStash<DropTargetComponent>();

            req_monsterDrag = World.GetRequest<ChangeMonsterDraggableStateRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateExit.publishedChanges)
            {
                if (isStateValid(evt.StateEntity))
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

        private bool isStateValid(Entity stateEntity)
        {
            if (!stash_battlePlanningState.Has(stateEntity)) { return false; }
            else { return true; }
        }
    }

}
