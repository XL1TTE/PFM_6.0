using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Features.DragAndDrop.Events{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DragEndedEvent : IEventData
    {
        public Entity DraggedEntity;
        public Entity DropTargetEntity;
        public bool WasSuccessful;
    }
}


