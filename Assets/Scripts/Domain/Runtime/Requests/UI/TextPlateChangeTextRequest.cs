using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Domain.UI.Tags;



namespace Domain.UI.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TextPlateChangeTextRequest : IRequestData
    {
        public PlateWithTextTag.Origin origin;
        public string message;
    }

}
