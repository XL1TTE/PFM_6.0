using Domain.HealthBars.Components;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Domain.HealthBars.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthBarRendererProvider : MonoProvider<HealthBarRenderer>
    {

    }
}
