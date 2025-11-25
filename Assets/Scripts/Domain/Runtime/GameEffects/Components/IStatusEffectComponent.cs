using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.GameEffects
{

    public interface IStatusEffectComponent : IComponent
    {
        [Serializable]
        class Stack
        {
            public int m_Duration;
            public int m_TurnsLeft;
            public int m_DamagePerTurn;
        }

        [property: SerializeField]
        List<Stack> m_Stacks { get; set; }
    }
}
