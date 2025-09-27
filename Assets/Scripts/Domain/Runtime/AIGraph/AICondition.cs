using Unity.IL2CPP.CompilerServices;
using System;
using Domain.Extentions;

namespace Domain.AIGraph
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AICondition
    {
        public AIConditionType Type;
        public ComparisonOperator Operator;
        public float FloatValue;
        public int IntValue;
        public string StringValue;
    }

    public enum AIConditionType : byte
    {
        MoveCompleted,
        MoveAnimating,
        NoAvaibleMoves,
    }

}
