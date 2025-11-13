using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Map.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MapTextEventEnterRequest : IRequestData {
        // id of an event that needs to be drawn in ui
        public string event_id;
    }
}