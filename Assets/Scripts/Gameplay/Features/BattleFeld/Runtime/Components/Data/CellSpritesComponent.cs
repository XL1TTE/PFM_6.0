using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Features.BattleField.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CellSpritesComponent : IComponent
    {
        public enum SpriteStates:byte{
            Default,
            Hovered,
            Highlighted
        }
        
        [HideInInspector] public Sprite EmptySprite;
        public Sprite HoverSprite;
        public Sprite HighlightedSprite;

        [HideInInspector] private SpriteStates _spriteState;
        [HideInInspector] public SpriteStates SpriteState {
            set {
                PreviousSpriteState = _spriteState;
                _spriteState = value;
            }
            get => _spriteState;
        }
        [HideInInspector] public SpriteStates PreviousSpriteState;
    }
}
