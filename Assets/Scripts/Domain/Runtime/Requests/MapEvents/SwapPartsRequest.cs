using Scellecs.Morpeh;
using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;

namespace Domain.MapEvents.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SwapPartsRequest : IRequestData
    {
        public Dictionary<BODYPART_SPECIFIED_TYPE,string> parts_type_with_id;
    }
}

