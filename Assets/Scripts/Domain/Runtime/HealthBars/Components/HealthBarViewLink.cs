using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.HealthBars.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HealthBarViewLink : IComponent
    {
        public HealthBarView Value;
    }


}
