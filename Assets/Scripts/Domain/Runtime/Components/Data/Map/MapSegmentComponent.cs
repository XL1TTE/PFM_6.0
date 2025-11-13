using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Map.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MapBGComponent : IComponent 
    {
        //public List<Sprite> sprites;
        public Sprite sprite;

        public int scale_x;

        public int pos_x;
        public int pos_y;

        public int layer;

        public BG_TYPE bg_type;
    }

    public enum BG_TYPE
    {
        START,
        END,
        MIDDLE,
        BG
    }
}