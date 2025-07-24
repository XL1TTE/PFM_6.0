
using ECS.Components;
using Scellecs.Morpeh;

namespace ECS.Systems{

    public sealed class CellPositionLogSystem : ISystem
    {
        public CellPositionLogSystem(float cooldown){
            _logCooldown = cooldown;
        }
        private float _logCooldown;
        private float _timer = 0;
        
        public World World { get; set; }

        private Filter _entities; 
        private Stash<CellPositionComponent> _cellPositions;

        public void OnAwake()
        {
            _entities = World.Filter
                .With<TagBattleFieldCell>()
                .With<CellPositionComponent>()
                .Build();
                
            _cellPositions = World.GetStash<CellPositionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if(_timer < _logCooldown){_timer += deltaTime; return; }
            
            foreach(var e in _entities){
                var pos = _cellPositions.Get(e);
                UnityEngine.Debug.Log($"Cell ({pos.grid_x}, {pos.grid_y}) have world position: ({pos.global_x}, {pos.global_y})");
            }
            _timer = 0;
        }

        public void Dispose()
        {
            
        }
    }
}
