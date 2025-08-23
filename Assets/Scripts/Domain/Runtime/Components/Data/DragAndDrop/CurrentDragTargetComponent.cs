using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Domain.DragAndDrop.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CurrentDragTargetComponent : IComponent
    {
        public Entity TargetEntity;
        public Vector3 ValidDropPosition;
        public bool IsValid;
    }
}


