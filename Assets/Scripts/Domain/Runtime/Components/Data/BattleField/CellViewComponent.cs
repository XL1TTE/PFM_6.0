using System;
using Scellecs.Morpeh;

namespace Domain.BattleField.Components
{
    [Serializable]
    public struct CellViewComponent : IComponent
    {
        public CellView Value;
    }
}
