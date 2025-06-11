using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.UnityComponents{
    [Serializable] public struct UnityTransformComponent: IComponent{
        public Transform transform;
    }
}
