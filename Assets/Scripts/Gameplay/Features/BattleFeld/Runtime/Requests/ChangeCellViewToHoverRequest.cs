using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using System.Collections;

namespace Gameplay.Features.BattleField.Requests{

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ChangeCellViewToHoverRequest : IRequestData
    {
        public enum HoverState:byte{Enabled, Disabled}
        public IEnumerable<Entity> Cells;
        public HoverState State;
        
    }
}


