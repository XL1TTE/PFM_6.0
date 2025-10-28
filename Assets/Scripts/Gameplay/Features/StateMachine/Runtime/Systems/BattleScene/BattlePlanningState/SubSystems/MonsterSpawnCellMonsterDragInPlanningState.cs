using Domain.BattleField.Tags;
using Domain.Components;
using Domain.CursorDetection.Components;
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Requests;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterSpawnCellMonsterDragInPlanningState : ISystem
    {
        public World World { get; set; }

        private Filter f_state;

        private Stash<BattlePlanningState> stash_planningState;

        private Filter _cellsWithMonsterUnderCursor;
        private Stash<TagOccupiedCell> stash_occupiedCell;
        private Stash<UnderCursorComponent> stash_underCursor;
        private Stash<TransformRefComponent> stash_transformRef;
        private Request<StartDragRequest> req_startDrag;

        private Filter _currentDrag;

        public void OnAwake()
        {
            f_state = SM.Value.Filter.With<BattlePlanningState>().Build();

            stash_planningState = SM.Value.GetStash<BattlePlanningState>();

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
            if (IsValid() == false) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                var cellUnderCursor = _cellsWithMonsterUnderCursor.First();
                var monster = stash_occupiedCell.Get(cellUnderCursor).m_Occupier;

                if (monster.Id == _currentDrag.FirstOrDefault().Id)
                {
                    return;
                }
                if (_currentDrag.IsEmpty() == false) { return; }

                var underCursorData = stash_underCursor.Get(cellUnderCursor);
                var monsterTranform = stash_transformRef.Get(monster).Value;
                req_startDrag.Publish(new StartDragRequest
                {
                    DraggedEntity = monster,
                    ClickWorldPos = underCursorData.HitPoint,
                    StartPosition = monsterTranform.position
                }, true);
            }
        }

        public void Dispose()
        {

        }

        private bool IsValid()
        {
            if (SM.IsStateActiveOptimized(f_state, stash_planningState, out var state))
            {
                return true;
            }
            return false;
        }
    }
}


