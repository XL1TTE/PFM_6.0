
using Scellecs.Morpeh;

namespace Gameplay.Features.BattleField.Events
{
    public struct CellUnoccupiedEvent: IEventData{
        public Entity CellEntity;
    }
}
