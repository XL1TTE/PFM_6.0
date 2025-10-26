using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;


namespace Domain.CursorDetection.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct OnCursorEnterEvent : IEventData
    {
        /// <summary>
        /// Entity which collider cursor entered.
        /// </summary>
        public Entity m_Entity;
    }
}

