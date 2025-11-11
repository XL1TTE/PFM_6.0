using Domain.Providers;
using Domain.UI.Components;

using Unity.IL2CPP.CompilerServices;

namespace Domain.UI.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TextMeshProRefComponentProvider : ComponentProvider<TextMeshProRefComponent>
    {

    }
}


