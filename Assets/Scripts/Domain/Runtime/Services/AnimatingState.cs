using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Services
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimatingState : IComponent
    {
        /// <summary>
        /// Status code of animation.
        /// </summary>
        public AnimatingStatus m_Status;

        /// <summary>
        /// Only used for per frame animations.
        /// </summary>
        public ushort m_CurrentFrame;

        /// <summary>
        /// Only for tween animations.
        /// </summary>
        public bool m_IsTweenInteractionFrame;
    }
}
