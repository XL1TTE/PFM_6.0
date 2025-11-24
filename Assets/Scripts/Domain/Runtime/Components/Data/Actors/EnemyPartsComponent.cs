using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EnemyPartsComponent : IComponent
    {
        public string m_LeftHandID;
        public string m_RightHandID;
        public string m_HeadID;
        public string m_TorsoID;
        public string m_LeftLeg;
        public string m_RightLeg;
    }
}


