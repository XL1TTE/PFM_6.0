
using Scellecs.Morpeh;

namespace Domain.BattleField.Requests
{
    public struct MarkMonsterSpawnCellsAsDropTargetRequest: IRequestData{
        
        public enum State: byte{
            Enable,
            Disable
        }
        public State state;
        public float DropRadius;
    }
}
