using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityComponent : IComponent { }

    public enum NodeType : byte { Immediate, WaitForAnimationFrame, WaitForTween, WaitForDamage, WaitForEffect, End }
    public enum ConditionType : byte { AnimationFrame, DamageDealt, EffectApplied, Custom }
}

