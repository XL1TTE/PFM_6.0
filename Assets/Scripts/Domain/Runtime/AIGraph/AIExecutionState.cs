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
    public struct AIExecutionState : IComponent
    {
        public enum MoveStatus : byte { None, Completed, Animating, Failed, NoTargets }

        public int CurrentNodeId;

        // Condition flags

        public MoveStatus MovementStatus;

        public bool AbilityTargetSelected;
        public bool AbilityCompleted;
        public bool CustomConditionMet;

        // Data for actions
        public Entity SelectedMoveTarget;
        public List<Entity> SelectedAbilityTargets;
        public string SelectedAbilityID;

        // Timers and counters
        public float ExecutionTimer;
        public int TurnCount;
    }
}
