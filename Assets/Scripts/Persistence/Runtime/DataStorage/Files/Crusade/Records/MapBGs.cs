
using Domain.Map.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence.DS
{
    public struct MapBGs : IDataStorageRecord
    {
        public List<MapBGWrapper> arr_bgs;
    }
    public struct MapBGWrapper
    {
        public Sprite sprite;

        public int scale_x;

        public int pos_x;
        public int pos_y;

        public int layer;

        public BG_TYPE bg_type;
    }
}
