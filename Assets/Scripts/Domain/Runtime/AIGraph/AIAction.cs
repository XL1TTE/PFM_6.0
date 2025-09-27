using Unity.IL2CPP.CompilerServices;
using System;

namespace Domain.AIGraph
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AIAction
    {
        public AIActionType Type;

        public MoveAnimationType MoveAnimation;

        public TargetType AbilityTargetType;
        public TargetPriority AbilityTargetPriority;
        public int MaxTargets;

        public int AbilityId;

        public float FloatParam;
        public int IntParam;
        public string StringParam;
    }

}
