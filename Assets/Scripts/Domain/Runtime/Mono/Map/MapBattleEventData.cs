using System.Collections.Generic;

namespace Domain.Monster.Mono
{

    public sealed class MapBattleEventData : IMapEventData
    {
        public EVENT_TYPE event_type => EVENT_TYPE.BATTLE;

#if UNITY_EDITOR
        public UnityEditor.PrefabAssetType level_prefab;
#endif
        public List<string> rewards_list;
        public int enemy_count;
    }
}
