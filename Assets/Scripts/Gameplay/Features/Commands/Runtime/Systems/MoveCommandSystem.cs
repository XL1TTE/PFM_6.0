using DG.Tweening;
using Domain.BattleField.Events;
using Domain.Commands.Components;
using Domain.Commands.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Commands{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveCommandSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<MoveToCellRequest> req_moveToCell;
        
        private Event<CellOccupiedEvent> evt_cellOccupied;
        
        private Stash<IsMoving> stash_isMoving;

        public void OnAwake()
        {
            req_moveToCell = World.GetRequest<MoveToCellRequest>();

            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

            stash_isMoving = World.GetStash<IsMoving>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_moveToCell.Consume()){

                evt_cellOccupied.NextFrame(new CellOccupiedEvent{
                    CellEntity = req.TargetCell,
                    OccupiedBy = req.Subject
                });

                stash_isMoving.Add(req.Subject);
                req.MoveSequence.onComplete += 
                    () => stash_isMoving.Remove(req.Subject); 
            }
        }

        public void Dispose()
        {

        }
    }
}


