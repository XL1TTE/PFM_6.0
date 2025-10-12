
using System.Collections.Generic;
using UnityEditor;

namespace Domain.Monster.Mono{
    
    public sealed class MapTextEventData : IMapEventData
    {
        public EVENT_TYPE event_type => EVENT_TYPE.TEXT;

        public string bg_sprite_path;
        public string string_message;
        public Dictionary<string,string> choices;
    } 
}
