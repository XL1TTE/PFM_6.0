using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.MapEvents.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TakeHPRequest : IRequestData
    {
        public uint amount_flat;
        public bool use_percent;
        public float amount_percent;
        public uint monster_count;
    }
}

