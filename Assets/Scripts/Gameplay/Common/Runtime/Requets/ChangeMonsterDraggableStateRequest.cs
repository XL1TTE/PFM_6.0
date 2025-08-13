
using Scellecs.Morpeh;

namespace Gameplay.Common.Requests
{
    public struct ChangeMonsterDraggableStateRequest: IRequestData{
        public enum State{
            Enabled,
            Disabled
        }
        
        public State state;
    }
}
