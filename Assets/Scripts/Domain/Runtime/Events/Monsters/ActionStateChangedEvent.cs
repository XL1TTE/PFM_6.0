using Scellecs.Morpeh;

namespace Domain.Monster.Events{
    
    public sealed class ActionStateChangedEvent: IEventData{
        public bool IsPerformingAction;
    }
    
}
