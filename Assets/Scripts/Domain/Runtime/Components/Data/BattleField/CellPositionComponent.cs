using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.BattleField.Components
{
    [Serializable]
    public struct PositionComponent : IComponent
    {
        public Vector2Int m_GridPosition;
        public Vector2 m_GlobalPosition;
    }
}
