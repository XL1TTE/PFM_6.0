
using System.Collections.Generic;

namespace Persistence.DS
{
    public struct MapNodes : IDataStorageRecord
    {
        public List<MapNodeWrapper> arr_nodes;
    }

    public struct MapNodeWrapper
    {
        // base id
        public byte node_id;


        // neighb
        public List<byte> node_neighbours;


        // position
        public int node_x;
        public int node_y;

        public int node_x_offset;
        public int node_y_offset;

        public int node_collumn;
        public int node_row;

        // event
        public string event_id;
        public EVENT_TYPE event_type;

        // tags
        //public bool current;
        public bool crossed;
        //public bool choosable;
    }
}
