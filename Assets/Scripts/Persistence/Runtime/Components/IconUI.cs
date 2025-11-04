using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Persistence.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct IconUI : IComponent
    {
        public IconUI(Sprite a_value)
        {
            m_Value = a_value;
        }
        public Sprite m_Value;
    }

}
