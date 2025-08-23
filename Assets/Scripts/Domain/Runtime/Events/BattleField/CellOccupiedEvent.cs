
using Scellecs.Morpeh;

namespace Domain.BattleField.Events
{
    public struct CellOccupiedEvent : IEventData{
        public Entity CellEntity;
        public Entity OccupiedBy;
    }
}
