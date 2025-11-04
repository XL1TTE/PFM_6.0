using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Services
{

    /// <summary>
    /// Request which should lead to animation play on subject entity.
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimatingRequest : IRequestData
    {

        public Entity m_Subject;

    }

    /// <summary>
    /// Request which should lead to animation play on subject entity.
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimateWithTweenRequest : IRequestData
    {
        public Entity m_Subject;

        /// <summary>
        /// Target entity, for animation that needs target point.
        /// </summary>
        public Entity m_Target;

        /// <summary>
        /// Let you to select concrete type of tween animation. 
        /// </summary>
        public TweenAnimations m_TweenAnimationCode;
    }
}
