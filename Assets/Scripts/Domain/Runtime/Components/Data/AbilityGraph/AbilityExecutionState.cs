using Domain.Services;
using Scellecs.Morpeh;

namespace Domain.AbilityGraph
{
    public struct AbilityExecutionState : IComponent
    {
        public int m_CurrentNodeId;

        // Condition flags
        public bool m_DamageDealt;
        public bool m_EffectApplied;
        public bool m_CustomConditionMet;

        // Data for conditions
        public int m_CurrentAnimationFrame;
        public float m_LastDamageAmount;
        public string m_LastAppliedEffectId;

        public AnimatingStatus m_AnimatingStatus;
        public bool m_IsTweenInteractionFrame;

        // Timers and counters
        public float m_ExecutionTimer;
        public int m_TurnCount;
    }
}


