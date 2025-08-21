using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.DragAndDrop.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterSpawnCellMonsterDragInPlanningState : ISystem
    {
        public World World { get; set; }
        
        private Filter _currentStateFilter;
        private Stash<CurrentStateComponent> stash_currentState;
        private Stash<BattlePlanningState> stash_planningState;
        
        
        private Filter _cellsWithMonsterUnderCursor;
        private Stash<TagOccupiedCell> stash_occupiedCell;
        private Stash<UnderCursorComponent> stash_underCursor;
        private Stash<TransformRefComponent> stash_transformRef;
        private Request<StartDragRequest> req_startDrag;
        
        private Filter _currentDrag;

        public void OnAwake()
        {
            _currentStateFilter = World.Filter
                .With<CurrentStateComponent>()
                .Build();
            stash_currentState = World.GetStash<CurrentStateComponent>();
            stash_planningState = World.GetStash<BattlePlanningState>();

            _cellsWithMonsterUnderCursor = World.Filter
                .With<UnderCursorComponent>()
                .With<TagMonsterSpawnCell>()
                .With<TagOccupiedCell>()
                .Build();
            stash_occupiedCell = World.GetStash<TagOccupiedCell>();
            stash_underCursor = World.GetStash<UnderCursorComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
            req_startDrag = World.GetRequest<StartDragRequest>();

            _currentDrag = World.Filter
                .With<DragStateComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_cellsWithMonsterUnderCursor.IsEmpty()) { return; }
            if (isStateValid() == false){return;}

            if(Input.GetMouseButtonDown(0)){        
                var cellUnderCursor = _cellsWithMonsterUnderCursor.First();
                var monster = stash_occupiedCell.Get(cellUnderCursor).Occupier;

                if(monster.Id == _currentDrag.FirstOrDefault().Id){
                    return;
                }

                var underCursorData = stash_underCursor.Get(cellUnderCursor);
                var monsterTranform = stash_transformRef.Get(monster).TransformRef;
                req_startDrag.Publish(new StartDragRequest{
                    DraggedEntity = monster,
                    ClickWorldPos = underCursorData.HitPoint,
                    StartPosition = monsterTranform.position
                });
            }
        }

        public void Dispose()
        {
            
        }
        
        private bool isStateValid(){
            var currentState = _currentStateFilter.FirstOrDefault();
            if(currentState.IsExist()){
                var state = stash_currentState.Get(currentState);
                if(stash_planningState.Has(state.Value)){
                    return true;
                }
            }
            return false;
        }
    }
}


