
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.DragAndDrop.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.DragAndDrop.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DropTargetMarkSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _monsterSpawnCells;
        private Request<MarkMonsterSpawnCellsAsDropTargetRequest> req_markSpawnCells;

        private Stash<DropTargetComponent> stash_dropTarget;

        public void OnAwake()
        {
            _monsterSpawnCells = World.Filter
                .With<TagMonsterSpawnCell>()
                .Build();

            req_markSpawnCells = World.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();
            
            stash_dropTarget = World.GetStash<DropTargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_markSpawnCells.Consume()){
                if(req.state == MarkMonsterSpawnCellsAsDropTargetRequest.State.Enable){
                    MarkAllMonsterSpawnCellsAsDropTargets(req.DropRadius);
                }
                else if(req.state == MarkMonsterSpawnCellsAsDropTargetRequest.State.Disable){
                    UnmarkAllMonsterSpawnCellsAsDropTargets();
                }
            }
        }

        public void Dispose()
        {
            _monsterSpawnCells.Dispose();
        }
        
        
        private void MarkAllMonsterSpawnCellsAsDropTargets(float dropRadius){
            foreach(var e in _monsterSpawnCells){
                if(!stash_dropTarget.Has(e)){
                    ref var c = ref stash_dropTarget.Add(e);
                    c.DropRadius = dropRadius;
                }
            }
        }
        
        private void UnmarkAllMonsterSpawnCellsAsDropTargets(){
            foreach (var e in _monsterSpawnCells)
            {
                if (stash_dropTarget.Has(e))
                {
                    stash_dropTarget.Remove(e);
                }
            }
        }
    }
}

