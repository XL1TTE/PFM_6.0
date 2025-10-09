using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Domain.HealthBars.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HealthBarRenderer : IComponent
    {
        [SerializeField] private Transform p_healthBarContainer;
        public Vector3 GetHealthBarWorldPosition() => p_healthBarContainer.position;
    }
}
