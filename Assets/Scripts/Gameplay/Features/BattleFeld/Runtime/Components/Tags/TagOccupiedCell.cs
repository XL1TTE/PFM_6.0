using System;
using Scellecs.Morpeh;

namespace Gameplay.Features.BattleField.Components
{
    [Serializable] public struct TagOccupiedCell : IComponent{
        public Entity Occupier;
    }
}
