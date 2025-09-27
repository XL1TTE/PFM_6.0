using UnityEngine;

namespace Domain.AIGraph
{

    public enum AIType : byte { Default, Melee, Ranged, Healer, Tank, Support }

    public enum AINodeType : byte { Immediate, WaitForMovement, WaitForAbilityCompletion, WaitForCustomCondition, End }

    public enum AIActionType : byte
    {
        TryToMove,
        SelectAbilityTarget,
        UseAbility,
    }

    public enum TargetPriority : byte
    {
        Closest,
        LowestHealth,
        Random
    }

    public enum TargetType : byte
    {
        Enemy,
        Ally,
        Self,
        Object,
        Any
    }

}
