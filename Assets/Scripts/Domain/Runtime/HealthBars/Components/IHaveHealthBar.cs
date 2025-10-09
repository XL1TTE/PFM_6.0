using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Domain.UI.Widgets;

namespace Domain.HealthBars.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct IHaveHealthBar : IComponent
    {
        public HealthBarView HealthBarPrefab;
    }
}
