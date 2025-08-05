using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.Features.DragAndDrop.Components{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct UnderCursorComponent : IComponent
    {
        public Vector3 HitPoint;
    }
}

