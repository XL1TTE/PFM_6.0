using Domain.Providers;
using Domain.TurnSystem.Components;
using Unity.IL2CPP.CompilerServices;

namespace Domain.TurnSystem.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnQueueAvatarProvider : ComponentProvider<AvatarUI>
    {

    }
}
