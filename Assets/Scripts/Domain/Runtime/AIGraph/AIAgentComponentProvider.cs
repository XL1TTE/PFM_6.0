using Unity.IL2CPP.CompilerServices;
using System;
using Domain.Providers;

namespace Domain.AIGraph
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AIAgentComponentProvider : ComponentProvider<AIAgentComponent>
    {
    }

}
