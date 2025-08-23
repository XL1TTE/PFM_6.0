
using Scellecs.Morpeh;

namespace Domain.DragAndDrop.Requests
{
    public struct ChangeMonsterDraggableStateRequest: IRequestData{
        public enum State{
            Enabled,
            Disabled
        }
        
        public State state;
    }
}
