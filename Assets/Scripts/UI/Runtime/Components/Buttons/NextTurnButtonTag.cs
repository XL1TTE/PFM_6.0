using Scellecs.Morpeh;
using UI.Mono.View;
using Unity.IL2CPP.CompilerServices;

namespace UI.Components{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct NextTurnButtonTag : IComponent
    {
        public NextTurnBtnView View;
    }
}


