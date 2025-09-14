using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityComponent : IComponent{}
    
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityCasterComponent : IComponent
    {
        public Entity caster;
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityTargetsComponent : IComponent
    {
        public int TargetCount;
        public Entity[] targets;
    }
    
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityExecutionGraph : IComponent
    {
        public List<ExecutionNode> Nodes;
        public int StartNodeId;
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ExecutionNode : IComponent
    {
        public int NodeId;
        public NodeType Type;
        public List<ActionData> Actions;
        public List<NodeTransition> Transitions;
    }
    public enum NodeType:byte { Immediate, WaitForAnimationFrame, WaitForTween, WaitForDamage, WaitForEffect, Conditional }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct NodeTransition : IComponent
    {
        public int TargetNodeId;
        public TransitionCondition Condition;
    }

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
    public enum ComparisonOperator:byte{LesserThen, GreaterThen, GreaterOrEqual, LesserOrEqual, Equals}
    
    public enum ConditionType:byte { AnimationFrame, DamageDealt, EffectApplied, Custom }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityIsExecutingTag : IComponent { }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityExecutionCompletedTag : IComponent { }
}

