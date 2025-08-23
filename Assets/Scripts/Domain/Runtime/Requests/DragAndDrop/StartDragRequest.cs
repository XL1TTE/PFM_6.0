using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Domain.DragAndDrop.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct StartDragRequest : IRequestData
    {
        public Entity DraggedEntity;
        public Vector3 ClickWorldPos;
        public Vector3 StartPosition;
    }
}


