using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Commands
{
    /// <summary>
    /// Event which notify that entity ended processing movement action. 
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MovementEnded : IEventData
    {
        /// <summary>
        /// Entity to which movement aplied.
        /// </summary>
        public Entity m_Subject;
    }
}
