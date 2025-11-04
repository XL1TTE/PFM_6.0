using System;
using Scellecs.Morpeh;
using TriInspector;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Stats.Components
{

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Health : IComponent
    {
        [ShowInInspector]
        private int m_Value;

        public Health(int value)
        {
            m_Value = value;
        }

        public void SetHealth(int a_value) => m_Value = a_value;
        public int GetHealth() => m_Value;


        public void ChangeHealth(int amount)
            => m_Value = Math.Max(0, m_Value + amount);

    }
}


