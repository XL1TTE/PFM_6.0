using Scellecs.Morpeh;

namespace Domain.AbilityGraph
{
    public struct AbilityExecutionState : IComponent
    {
        public int CurrentNodeId;

        // Condition flags
        public bool AnimationNotExist;
        public bool AnimationFrameReached;
        public bool DamageDealt;
        public bool EffectApplied;
        public bool CustomConditionMet;

        // Data for conditions
        public int CurrentAnimationFrame;
        public float LastDamageAmount;
        public string LastAppliedEffectId;
        public string LastAnimationEvent;

        // Timers and counters
        public float ExecutionTimer;
        public int TurnCount;
    }
}

