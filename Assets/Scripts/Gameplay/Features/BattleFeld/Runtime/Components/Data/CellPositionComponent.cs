using System;
using Scellecs.Morpeh;

namespace Gameplay.Features.BattleField.Components
{
    [Serializable]
    public struct CellPositionComponent: IComponent{
        public int grid_x;
        public int grid_y;
        
        public float global_x;
        public float global_y; 
    }
}
