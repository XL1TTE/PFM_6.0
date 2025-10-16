using DG.Tweening;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Services
{
    /// <summary>
    /// Request which should lead to Subject be moved to Target Cell with provided animation.
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MoveToCellRequest : IRequestData
    {
        /// <summary>
        /// Animation with which subjet will be moved.
        /// </summary>
        public Sequence m_MoveSequence;
        /// <summary>
        /// Entity that will be moved.
        /// </summary>
        public Entity m_Subject;
        /// <summary>
        /// Cell to which position subject will be moved.
        /// </summary>
        public Entity m_TargetCell;
    }
}
