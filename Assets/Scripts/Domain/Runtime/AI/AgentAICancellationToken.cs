using System.Threading;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AgentAICancellationToken : IComponent
    {
        public CancellationTokenSource m_TokenSource;

    }
}


