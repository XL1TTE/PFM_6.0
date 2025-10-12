
using System.Collections.Generic;
using UnityEditor;

namespace Domain.Monster.Mono{
    
    public sealed class MapBattleEventData : IMapEventData
    {
        public EVENT_TYPE event_type => EVENT_TYPE.BATTLE;

        public PrefabAssetType level_prefab;
        public List<string> rewards_list;
        public int enemy_count;
    } 
}
