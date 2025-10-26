using Domain.Abilities.Mono;
using Domain.Abilities.Tags;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityButtonTagProvider : MonoProvider<AbiltiyButtonTag>
    {
    }

}
