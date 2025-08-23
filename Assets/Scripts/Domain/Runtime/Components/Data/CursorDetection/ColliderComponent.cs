using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


namespace Domain.CursorDetection.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ColliderComponent : IComponent
    {
        public Vector2 size;
        public Vector2 offset; 
    }
}

