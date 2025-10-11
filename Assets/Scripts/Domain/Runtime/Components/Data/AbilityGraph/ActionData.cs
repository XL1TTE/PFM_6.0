using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.AbilityGraph
{
    public struct ActionData : IComponent
    {
        public ActionType Type;

        // Data
        public bool OnSelf;
        
        /// <summary>
        /// Provide -1 if you wanna use all tagets at once.
        /// </summary>
        public int TargetIndex;
        public short MinDamageValue;
        public short MaxDamageValue;
        public DamageType DamageType;
        public string EffectID;
        /// <summary>
        /// Provide -1 for permanent effects;
        /// </summary>
        public short EffectDurationInTurns;
        public string VfxRecordId;
        public string SfxClipPath;
        public string AnimationName;
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

