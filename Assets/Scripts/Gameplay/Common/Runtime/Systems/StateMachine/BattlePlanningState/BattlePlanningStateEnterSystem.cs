using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Gameplay.Features.DragAndDrop.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateEnterSystem : ISystem
    {
        public World World { get; set; }
        private Filter _monsterSpawnCells;


        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattlePlanningState> stash_battlePlanning;
        private Stash<DropTargetComponent> stash_dropTarget;


        public void OnAwake()
        {
            _monsterSpawnCells = World.Filter
                .With<TagMonsterSpawnCell>()
                .Build();

            evt_onStateEnter = World.GetEvent<OnStateEnterEvent>();

            stash_battlePlanning = World.GetStash<BattlePlanningState>();
            stash_dropTarget = World.GetStash<DropTargetComponent>();
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
        
        private void Enter(Entity stateEntity){
            /* ############################################## */
            /*           Pre-spawn monsters request           */
            /* ############################################## */

            var genMonsterReq = World.Default.GetRequest<GenerateMonstersRequest>();

            genMonsterReq.Publish(new GenerateMonstersRequest
            {
                MosntersCount = 2
            }, true);

            /* ############################################## */
            /*         Hightlight monster spawn cells         */
            /* ############################################## */

            var highlightMonsterSpawnCellsReq = World.Default.GetRequest<EnableMonsterSpawnCellsHighlightRequest>();

            highlightMonsterSpawnCellsReq.Publish(new EnableMonsterSpawnCellsHighlightRequest { }, true);

            var req_markMonsterSpawnCellsAsDropTargets =
                World.Default.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest { DropRadius = 1.0f }, true);
        }
        
        private bool isStateValid(Entity stateEntity){
            if(!stash_battlePlanning.Has(stateEntity)){return false;}
            else{return true;}
        }
    }
}

