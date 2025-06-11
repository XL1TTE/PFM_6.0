using System;
using Scellecs.Morpeh;

namespace ECS.Components{
    [Serializable]
    public struct BattleFieldCellPosition: IComponent{
        public int x;
        public int y; 
    }
}
