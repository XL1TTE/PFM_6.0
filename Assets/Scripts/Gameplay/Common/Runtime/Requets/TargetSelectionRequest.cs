using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Common.Requests{

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TargetSelectionRequest : IRequestData
    {
        public enum SelectionType:byte{Enemy,Cell}
        
        public int RequestID;
        public UInt16 TargetCount;
        public List<Entity> AllowedTargets;
        public SelectionType Type;
    }
}


