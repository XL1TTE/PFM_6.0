using DG.Tweening;
using Domain.Services;
using Scellecs.Morpeh;

namespace Domain.AbilityGraph
{
    public struct ActionData : IComponent
    {
        public ActionType m_Type;

        // Data
        public bool m_OnSelf;

        /// <summary>
        /// Provide -1 if you wanna use all tagets at once.
        /// </summary>
        public int m_TargetIndex;
        public short m_MinDamageValue;
        public short m_MaxDamageValue;
        public DamageType m_DamageType;
        public string m_EffectID;
        /// <summary>
        /// Provide -1 for permanent effects;
        /// </summary>
        public short m_EffectDurationInTurns;


        public string m_VfxRecordId;
        public string m_SfxClipPath;


        public string m_AnimationName;
        public TweenAnimations m_TweenAnimationCode;
    }



    public enum DamageType : byte { Physical, Poison }

    public enum ActionType : byte
    {
        PlayAnimation,
        PlayTween,
        DealDamage,
        ApplyEffect,
        TurnAround,
        ConsumeMovementAction,
        ConsumeInteractionAction,
        SpawnVfx,
        PlaySfx,
    }
}

