
using Scellecs.Morpeh;

namespace ECS.Events
{
    public struct CellUnoccupiedEvent: IEventData{
        public Entity CellEntity;
    }
}
