using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System;

namespace Domain.AIGraph
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AgentTargetSelectionRequest : IRequestData
    {
        public Entity AgentEntity;
        public int MaxTargets;
    }

}
