

using Domain.Abilities;
using Gameplay.TargetSelection;
using Scellecs.Morpeh;
using UnityEngine;

namespace Persistence.DB
{
    public struct AbilityDefenition : IComponent
    {
        public Vector2Int[] m_Shifts;
        public TargetSelectionTypes m_TargetType;
        public Ability m_Ability;
    }
}
