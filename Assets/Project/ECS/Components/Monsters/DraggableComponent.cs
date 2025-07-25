
using System;
using Project.ObjectInteractions;
using Scellecs.Morpeh;

namespace ECS.Components.Monsters{
    [Serializable]
    public struct DraggableComponent: IComponent{
        public IDraggable Draggable; 
    }
    
}
