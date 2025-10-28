using System;
using System.Collections.Generic;
using Gameplay.TargetSelection;
using UnityEngine;

namespace Domain.Abilities.Components
{
    [Serializable]
    public sealed class AbilityData
    {
        /// <summary>
        /// Ability itself.
        /// </summary>
        public Ability m_Value;
        public string m_AbilityTemplateID;
        public TargetSelectionTypes m_TargetType;
        public IEnumerable<Vector2Int> m_Shifts;
        public Sprite m_Icon;
    }
}


