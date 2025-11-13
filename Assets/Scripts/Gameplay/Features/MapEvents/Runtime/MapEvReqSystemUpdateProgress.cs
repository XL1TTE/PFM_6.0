using Domain.Map.Components;
using Domain.Map.Requests;
using Game;
using Persistence.DS;
using Scellecs.Morpeh;

namespace Gameplay.Map.Systems
{
    public sealed class MapEvReqSystemUpdateProgress : ISystem
    {
        public World World { get; set; }

        public Request<MapUpdateProgress> req_update;
        private Request<MapUpdateVisualsRequest> req_update_vis;
        private Filter nodesFilter;
        private Filter nodeCurrentFilter;

        private Stash<TagMapNodeUsed> nodeUsedStash;
        private Stash<TagMapNodeCurrent> nodeCurrentStash;
        private Stash<TagMapNodeChoosable> nodeChoosableStash;

        private Stash<MapNodePositionComponent> nodePosStash;
        private Stash<MapNodeIdComponent> nodeIdStash;
        private Stash<MapNodeNeighboursComponent> nodeNeighbStash;

        public void OnAwake()
        {
            World = ECS_Main_Map.m_mapWorld;

            req_update = World.GetRequest<MapUpdateProgress>();
            req_update_vis = World.GetRequest<MapUpdateVisualsRequest>();

            nodesFilter = World.Filter.With< MapNodeIdComponent >().Build();
            nodeCurrentFilter = World.Filter.With<TagMapNodeCurrent>().Build();

            nodeUsedStash = World.GetStash<TagMapNodeUsed>();
            nodeCurrentStash = World.GetStash<TagMapNodeCurrent>();
            nodeChoosableStash = World.GetStash<TagMapNodeChoosable>();

            nodePosStash = World.GetStash<MapNodePositionComponent>();
            nodeIdStash = World.GetStash<MapNodeIdComponent>();
            nodeNeighbStash = World.GetStash<MapNodeNeighboursComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_update.Consume())
            {

                //Entity curr_node = nodeCurrentFilter.FirstOrDefault();
                Entity curr_node;

                // Сначала сбросить нынешний нод
                //if (nodeCurrentStash.Has(curr_node))
                if (nodeCurrentFilter.IsNotEmpty())
                {
                    curr_node = nodeCurrentFilter.FirstOrDefault();
                    nodeUsedStash.Add(curr_node);
                    nodeCurrentStash.Remove(curr_node);


                    var curr_id = nodeIdStash.Get(curr_node).node_id;
                    ref var crusadeNodes = ref DataStorage.GetRecordFromFile<Crusade, MapNodes>();
                    for (int i = 0; i < crusadeNodes.arr_nodes.Count; i++)
                    {
                        var n_wrap = crusadeNodes.arr_nodes[i];
                        if (curr_id == crusadeNodes.arr_nodes[i].node_id)
                        {
                            n_wrap.crossed = true;
                            crusadeNodes.arr_nodes[i] = n_wrap;
                            break;
                        }
                    }
                }
                else
                {
                    curr_node = nodeCurrentFilter.FirstOrDefault();
                }

                // Потом сбросить выбираемые ноды
                foreach (var entity in nodesFilter)
                {
                    if (nodeChoosableStash.Has(entity))
                    {
                        nodeChoosableStash.Remove(entity);
                    }

                    // одновременно можно найти и новый нынешний
                    ref var id = ref nodeIdStash.Get(entity);
                    if (id.node_id == req.end_node)
                    {
                        nodeCurrentStash.Add(entity);
                        curr_node = entity;
                    }
                }


                // И вот после этого уже можно создавать новые выбираемые ноды
                ref var nodeCurrNeighbComponent = ref nodeNeighbStash.Get(curr_node);
                ref var nodeCurrPosComponent = ref nodePosStash.Get(curr_node);

                int tmp_neighb_count = nodeCurrNeighbComponent.node_neighbours.Count;
                foreach (var entity in nodesFilter)
                {
                    ref var nodeIdComponent = ref nodeIdStash.Get(entity);
                    if (nodeCurrNeighbComponent.node_neighbours.Contains(nodeIdComponent.node_id))
                    {
                        ref var nodePosComponent = ref nodePosStash.Get(entity);
                        if (nodePosComponent.node_collumn > nodeCurrPosComponent.node_collumn)
                        {
                            nodeChoosableStash.Add(entity);
                        }

                        tmp_neighb_count--;
                    }

                    if (tmp_neighb_count <= 0) { break; }
                }



                req_update_vis.Publish(new MapUpdateVisualsRequest { } );
            }

        }

        public void Dispose()
        {
            //Debug.Log("NodeDrawSys is Disposing");
            //throw new System.NotImplementedException();
        }

    }

}