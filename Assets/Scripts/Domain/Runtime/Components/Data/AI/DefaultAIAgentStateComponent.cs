using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AI.Components{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DefaultAIAgentStateComponent : IComponent
    {
        public enum AgentState:byte{Idle, Attacking, Moving}
        public AgentState Value;
    }

}
