using System;
using Scellecs.Morpeh;

namespace Domain.BattleField.Tags
{
    [Serializable]
    public struct TagBurnedCell : IComponent
    {
        public int m_Damage;
    }
}
