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
        public enum TargetSelectStatus : byte { None, Successful, NoTargets }
        public enum AbilityExecuteStatus : byte { None, InProcess, Failed, Completed }

        public int CurrentNodeId;


        public MoveStatus MovementStatus;
        public TargetSelectStatus TargetSelectionStatus;
        public AbilityExecuteStatus AbilityExecutionStatus;


        // Data for actions
        public Entity SelectedMoveTarget;
        public List<Entity> SelectedTargets;
        public string SelectedAbilityID;

        // Timers and counters
        public float ExecutionTimer;
        public int TurnCount;
    }
}
