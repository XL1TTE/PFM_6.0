using DG.Tweening;
using Domain.BattleField.Events;
using Domain.Services;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System.Linq;
using Scellecs.Morpeh.Collections;
using CodiceApp.EventTracking.Plastic;
using System.Collections.Generic;

namespace Gameplay.Commands
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveToCellServiceSystem : ISystem
    {
        public World World { get; set; }

        private Request<MoveToCellRequest> req_moveToCell;

        private Event<CellOccupiedEvent> evt_cellOccupied;
        private Event<MovementStarted> evt_moveStarted;
        private Event<MovementEnded> evt_moveEnded;

        public void OnAwake()
        {
            req_moveToCell = World.GetRequest<MoveToCellRequest>();

            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();
            evt_moveStarted = World.GetEvent<MovementStarted>();
            evt_moveEnded = World.GetEvent<MovementEnded>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_moveToCell.Consume())
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
            }
        }

        public void Dispose()
        {

        }
    }
}


