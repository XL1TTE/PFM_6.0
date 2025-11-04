using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Services
{
    /// <summary>
    /// Event which notify that entity plays some animation. 
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimatingStarted : IEventData
    {
        /// <summary>
        /// Animating entity.
        /// </summary>
        public Entity m_Subject;
    }
}
