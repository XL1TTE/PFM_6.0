using System;
using Domain.BattleField.Mono;
using Scellecs.Morpeh;

namespace Domain.BattleField.Components
{
    [Serializable]
    public struct CellViewComponent : IComponent
    {
        public BattleFieldCellView Value;
    }
}
