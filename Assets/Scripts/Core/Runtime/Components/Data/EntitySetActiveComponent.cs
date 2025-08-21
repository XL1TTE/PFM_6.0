using Core.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Core.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EntitySetActiveComponent : IComponent 
    {
        public SetActiveController controller;
    } 
}

