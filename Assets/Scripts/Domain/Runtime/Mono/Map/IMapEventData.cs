
using System.Collections.Generic;

namespace Domain.Monster.Mono{

    // this data component works like a tag for usage inside unity to process map event data 
    public interface IMapEventData
    {
        public EVENT_TYPE event_type { get; }
    }
}
