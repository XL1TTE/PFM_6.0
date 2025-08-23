
using Scellecs.Morpeh;

namespace Domain.BattleField.Events
{
    public struct CellUnoccupiedEvent: IEventData{
        public Entity CellEntity;
    }
}
