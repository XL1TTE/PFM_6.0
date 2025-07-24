
using Scellecs.Morpeh;

namespace ECS.Events
{
    public struct CellOccupiedEvent : IEventData{
        public Entity CellEntity;
    }
}
