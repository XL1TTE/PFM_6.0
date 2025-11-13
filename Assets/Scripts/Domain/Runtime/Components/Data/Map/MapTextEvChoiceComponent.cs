using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Map.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MapTextEvChoiceComponent : IComponent 
    {
        // this is an ID from the array of all answers of this text event
        public int count_id;
    }
}