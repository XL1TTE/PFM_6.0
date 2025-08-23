using System;
using Scellecs.Morpeh;

namespace Domain.BattleField.Tags
{
    [Serializable] public struct TagOccupiedCell : IComponent{
        public Entity Occupier;
    }
}
