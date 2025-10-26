

using Domain.Ability;
using Scellecs.Morpeh;
using UnityEngine;

namespace Persistence.DB
{
    public struct AbilityData : IComponent
    {
        public Vector2Int[] m_Shifts;
        public Ability m_Ability;
    }
}
