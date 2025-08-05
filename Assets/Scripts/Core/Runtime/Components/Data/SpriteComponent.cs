
using Core.Utilities.Wrappers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Core.Components{

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpriteComponent: IComponent{
        public Sprite Sprite; 
    }
    
}
