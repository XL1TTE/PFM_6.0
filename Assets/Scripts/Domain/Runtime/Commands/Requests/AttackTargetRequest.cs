using DG.Tweening;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Commands
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AttackTargetRequest : IRequestData
    {
        public Sequence AttackSequence;
        public Entity Attacker;
    }
}
