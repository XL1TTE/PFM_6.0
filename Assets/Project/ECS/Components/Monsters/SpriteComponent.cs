
using System;
using Scellecs.Morpeh;
using UnityUtilities;

namespace ECS.Components.Monsters{
    
    [Serializable]
    public struct SpriteComponent: IComponent{
        public Sprite Sprite; 
    }
    
}
