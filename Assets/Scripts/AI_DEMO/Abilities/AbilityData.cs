using System;
using System.Collections.Generic;
using Gameplay.TargetSelection;
using UnityEngine;

namespace Domain.Abilities.Components
{

    public enum AbilityTags : short
    {
        DAMAGE,
        HEAL,
        EFFECT,
        DEBUFF
    }

    public enum AbilityType : byte
    {
        MOVEMENT,
        INTERACTION,
        ROTATE,
        ALL
    }

    [Serializable]
    public sealed class AbilityData
    {
        /// <summary>
        /// Ability itself.
        /// </summary>
        public Ability m_Value;
        public string m_AbilityTemplateID;
        public TargetSelectionTypes m_TargetType;
        public List<Vector2Int> m_Shifts;
        public Sprite m_Icon;
        public List<AbilityTags> m_Tags;
        public AbilityType m_AbilityType;
    }
}


