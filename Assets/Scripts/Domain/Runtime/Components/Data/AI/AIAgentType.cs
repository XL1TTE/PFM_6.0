using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AI.Components{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AIAgentType : IComponent
    {
        public enum AgentType:byte{Default, Tank, Healer}
        public AgentType Value;
    }

}
