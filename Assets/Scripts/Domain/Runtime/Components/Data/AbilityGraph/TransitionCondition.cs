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
        public ConditionType m_Type;
        public int m_IntValue;
        public float m_FloatValue;
        public string m_StringValue;
        public ComparisonOperator m_Operator;
    }
}

