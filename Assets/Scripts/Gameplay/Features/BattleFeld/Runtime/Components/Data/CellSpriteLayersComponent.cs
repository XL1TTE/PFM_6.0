using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Features.BattleField.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CellSpriteLayersComponent : IComponent
    {
        public SpriteRenderer HoverLayer;
        public SpriteRenderer SelectedLayer;
        public SpriteRenderer PointerLayer;
    }
}
