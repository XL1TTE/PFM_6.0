using DG.Tweening;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Commands.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MoveToCellRequest : IRequestData
    {
        public Sequence MoveSequence;
        public Entity Subject;
        public Entity TargetCell;
    }
}
