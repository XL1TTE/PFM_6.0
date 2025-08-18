using System;
using Scellecs.Morpeh;

namespace Gameplay.Features.BattleField.Components
{
    [Serializable]
    public struct GridPosition: IComponent{
        public int grid_x;
        public int grid_y;
    }
}
