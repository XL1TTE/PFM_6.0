using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Domain.Monster.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PartsOffsets : IComponent
    {
        public Vector2 BodyOffset;
        public Vector2 NearArmOffset;
        public Vector2 FarArmOffset;
        public Vector2 HeadOffset;
        public Vector2 NearLegOffset;
        public Vector2 FarLegOffset;
    }
}


