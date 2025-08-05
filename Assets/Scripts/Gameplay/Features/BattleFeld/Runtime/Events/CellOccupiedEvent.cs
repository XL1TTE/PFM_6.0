
using Scellecs.Morpeh;

namespace Gameplay.Features.BattleField.Events
{
    public struct CellOccupiedEvent : IEventData{
        public Entity CellEntity;
        public Entity OccupiedBy;
    }
}
