
using System.Collections.Generic;

namespace Persistence.DS
{
    public struct CrusadeState : IDataStorageRecord
    {
        public byte curr_node_id;

        public STAGES curr_stage;

        public List<STAGES> passed_stages;

        public CRUSADE_STATE crusade_state;
    }

    public enum CRUSADE_STATE
    {
        NONE,
        CHOOSING,
        TEXT_EVENT,
        BATTLE,
        TUTORIAL
    }
}
