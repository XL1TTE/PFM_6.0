using System;
using Scellecs.Morpeh;

namespace ECS.Components{
    [Serializable]
    public struct BattleFieldCellPosition: IComponent{
        public int grid_x;
        public int grid_y;
        
        public float global_x;
        public float global_y; 
    }
}
