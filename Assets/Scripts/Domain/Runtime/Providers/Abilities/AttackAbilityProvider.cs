using Domain.Abilities.Components;
using Domain.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackAbilityProvider : ComponentProvider<AbilitiesComponent>
    {
    }

}
