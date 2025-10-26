

using Domain.Abilities;
using Scellecs.Morpeh;
using UnityEngine;

namespace Persistence.DB
{
    public struct AbilityDefenition : IComponent
    {
        public Vector2Int[] m_Shifts;
        public Ability m_Ability;
    }
}
