using DG.Tweening;
using Domain.BattleField.Events;
using Domain.Services;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.Commands
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MovementServiceSystem : ISystem
    {
        public World World { get; set; }

        private Request<MoveToCellRequest> req_moveToCell;
        private Request<TurnAroundRequest> req_TurnAround;

        private Event<CellOccupiedEvent> evt_cellOccupied;
        private Event<MovementStarted> evt_moveStarted;
        private Event<MovementEnded> evt_moveEnded;
        private Stash<LookDirection> stash_LookDirection;

        public void OnAwake()
        {
            req_moveToCell = World.GetRequest<MoveToCellRequest>();
            req_TurnAround = World.GetRequest<TurnAroundRequest>();

            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();
            evt_moveStarted = World.GetEvent<MovementStarted>();
            evt_moveEnded = World.GetEvent<MovementEnded>();

            stash_LookDirection = World.GetStash<LookDirection>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_moveToCell.Consume())
            {
                DoMoveToCell(req);
            }
            foreach (var req in req_TurnAround.Consume())
            {
                DoTurnAround(req);
            }
        }

        private void DoTurnAround(TurnAroundRequest req)
        {
            var subject = req.m_Subject;
            if (stash_LookDirection.Has(subject) == false) { return; }

            ref var lookDir = ref stash_LookDirection.Get(subject);

            switch (lookDir.m_Value)
            {
                case Directions.LEFT:
                    lookDir.m_Value = Directions.RIGHT;
                    break;
                case Directions.RIGHT:
                    lookDir.m_Value = Directions.LEFT;
                    break;
            }
        }

        private void DoMoveToCell(MoveToCellRequest req)
        {
            evt_cellOccupied.NextFrame(new CellOccupiedEvent
            {
                CellEntity = req.m_TargetCell,
                OccupiedBy = req.m_Subject
            });

            evt_moveStarted.NextFrame(new MovementStarted
            {
                m_Subject = req.m_Subject
            });

            req.m_MoveSequence.onComplete +=
                () => evt_moveEnded.NextFrame(new MovementEnded
                {
                    m_Subject = req.m_Subject
                });
            req.m_MoveSequence.Play();
        }

        public void Dispose()
        {

        }
    }
}


