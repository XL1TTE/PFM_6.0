using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Commands
{
    /// <summary>
    /// Event which notify that entity start processing movement action. 
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MovementStarted : IEventData
    {
        /// <summary>
        /// Entity to which movement will be aplied.
        /// </summary>
        public Entity m_Subject;
    }
}
