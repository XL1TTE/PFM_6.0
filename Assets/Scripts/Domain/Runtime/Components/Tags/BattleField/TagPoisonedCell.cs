using System;
using Scellecs.Morpeh;

namespace Domain.BattleField.Tags
{
    [Serializable]
    public struct TagPoisonedCell : IComponent
    {
        public int m_Damage;
    }
}
