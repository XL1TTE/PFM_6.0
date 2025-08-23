using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

namespace Domain.BattleField.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CellColorChangeRequest : IRequestData
    {
        public List<Entity> Cells;
        public string ColorHex;
    }
}


