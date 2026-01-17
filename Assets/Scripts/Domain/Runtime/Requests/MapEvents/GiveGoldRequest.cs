using Domain.Map.Mono;
using Scellecs.Morpeh;
using System;
using Unity.IL2CPP.CompilerServices;

namespace Domain.MapEvents.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct GiveGoldRequest : IRequestData
    {
        public uint amount;

        public static explicit operator GiveGoldRequest(CHOICE_SCRIPT_TYPE v)
        {
            throw new NotImplementedException();
        }
    }
}

