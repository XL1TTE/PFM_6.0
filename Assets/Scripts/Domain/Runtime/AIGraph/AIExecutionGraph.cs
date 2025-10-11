using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;

namespace Domain.AIGraph
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AIExecutionGraph : IComponent
    {
        public List<AINode> Nodes;
    }

}
