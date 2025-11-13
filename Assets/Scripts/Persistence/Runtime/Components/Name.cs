using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Persistence.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Name : IComponent
    {
        public Name(string a_value)
        {
            m_Value = a_value;
        }
        public string m_Value;
    }

}
