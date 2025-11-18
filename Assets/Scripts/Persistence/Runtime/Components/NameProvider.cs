using Domain.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Persistence.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class NameProvider : ComponentProvider<Name>
    {

    }

}
