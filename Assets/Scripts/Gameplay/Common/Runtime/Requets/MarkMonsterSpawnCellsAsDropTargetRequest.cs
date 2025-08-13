
using Scellecs.Morpeh;

namespace Gameplay.Common.Requests
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
