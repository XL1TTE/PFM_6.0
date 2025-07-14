using System;
using Scellecs.Morpeh;

namespace ECS.Components{
    [Serializable]
    public struct BattleFieldCellPosition: IComponent{
        public int grid_x;
        public int grid_y;
        
        public int global_x;
        public int global_y; 
    }
}
