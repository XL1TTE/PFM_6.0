using Domain.Monster.Tags;
using Domain.Providers;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Monster.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TagMonsterProvider : ComponentProvider<TagMonster> { }
}

