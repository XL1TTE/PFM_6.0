using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.AbilityGraph
{
    public struct ActionData : IComponent
    {
        public ActionType Type;

        // Data
        public int TargetIndex;
        public int MinDamageValue;
        public int MaxDamageValue;
        public DamageType DamageType;
        public string EffectTemplateId;
        public string VfxRecordId;
        public string SfxClipPath;
        public string AnimationName;
        public string SelectionType;
        public float SelectionRange;
    }
    
    public enum DamageType:byte{Physical, Poison}

    public enum ActionType:byte
    {
        PlayAnimation,
        DealDamage,
        ApplyEffect,
        SpawnVfx,
        PlaySfx,
    }
}

