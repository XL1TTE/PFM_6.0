using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TransitionCondition : IComponent
    {
        public ConditionType Type;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public ComparisonOperator Operator;
    }
}

