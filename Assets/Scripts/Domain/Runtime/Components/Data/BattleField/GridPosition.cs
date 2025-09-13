using System;
using System.Numerics;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.BattleField.Components
{
    [Serializable]
    public struct GridPosition: IComponent{
        public Vector2Int Value;
    }
}
