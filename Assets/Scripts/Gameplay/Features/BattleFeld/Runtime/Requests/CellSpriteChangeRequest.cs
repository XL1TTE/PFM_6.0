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
    public struct CellSpriteChangeRequest : IRequestData
    {
        public enum SpriteType:byte{
            Default,
            Previous,
            Hover,
            Highlighted
        }
        public IEnumerable<Entity> Cells;
        public SpriteType Sprite;
    }
}


