using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components.Monsters{
    
    [Serializable]
    public struct TransformComponent: IComponent{
        public Transform Transform; 
    }
    
}
