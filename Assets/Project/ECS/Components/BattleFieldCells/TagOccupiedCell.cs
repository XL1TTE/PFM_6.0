using System;
using Scellecs.Morpeh;

namespace ECS.Components{
    [Serializable] public struct TagOccupiedCell : IComponent{
        public Entity Occupier;
    }
}
