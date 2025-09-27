using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System;

namespace Domain.AIGraph
{
    public enum MoveAnimationType : byte { Chess, Linear }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AgentMoveRequest : IRequestData
    {

        public Entity AgentEntity;
        public MoveAnimationType Aniamtion;
    }

}
