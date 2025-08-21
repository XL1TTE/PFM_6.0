using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

namespace Gameplay.Features.BattleField.Requests{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ChangeCellViewToPointerRequest : IRequestData
    {
        public enum PointerState : byte{Enabled, Disabled}
        public IEnumerable<Entity> Cells;
        public PointerState State;
        
    }
}


