using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

namespace Domain.BattleField.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ChangeCellViewToSelectRequest : IRequestData
    {
        public enum SelectState : byte { Enabled, Disabled }
        public IEnumerable<Entity> Cells;
        public SelectState State;
        public string test;
    }
}


