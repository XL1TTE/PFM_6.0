using Domain.UI.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.UI.Tags{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ExitPlanningStageButtonTag : IComponent
    {
        public ExitPlanningStageBtnView View; 
    }
}


