using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Domain.BattleField.Events
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EntityCellPositionChangedEvent : IEventData
    {
        public Entity m_Subject;
        public Entity m_pCell;
        public Entity m_nCell;
    }
}


