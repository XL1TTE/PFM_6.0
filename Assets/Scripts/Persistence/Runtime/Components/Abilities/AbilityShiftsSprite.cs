using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;


namespace Persistence.DB
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityShiftsSprite : IComponent
    {
        //public AbilityShiftsSprite(Sprite a_value)
        //{
        //    m_Value = a_value;
        //}
        public Sprite m_Value;
    }
}