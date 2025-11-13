using Domain.Map.Components;
using Domain.Map.Events;
using Persistence.DS;
using Scellecs.Morpeh;

namespace Gameplay.Map.Systems
{
    public sealed class MapNodeClickBossWaitSystem : ISystem
    {
        public World World { get; set; }

        private Event<MapNodeClickedEvent> ev_clicked_node;

        private Stash<MapNodeEventId> nodeEvIDsStash;
        private Stash<MapNodeEventType> nodeTypesStash;
        private Stash<MapNodeIdComponent> nodeIDsStash;
        public void OnAwake()
        {
            ev_clicked_node = World.GetEvent<MapNodeClickedEvent>();

            nodeEvIDsStash = World.GetStash<MapNodeEventId>();
            nodeTypesStash = World.GetStash<MapNodeEventType>();
            nodeIDsStash = World.GetStash<MapNodeIdComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in ev_clicked_node.publishedChanges)
            {
                if (evt.node_entity != null)
                {
                    var ent = evt.node_entity;

                    ref var mapNodeEvTypeComponent = ref nodeTypesStash.Get(ent);

                    if (mapNodeEvTypeComponent.event_type == EVENT_TYPE.BOSS)
                    {
                        // Save Current State
                        ref var crusadeState = ref DataStorage.GetRecordFromFile<Crusade, CrusadeState>();
                        crusadeState.curr_node_id = nodeIDsStash.Get(ent).node_id;
                        crusadeState.crusade_state = CRUSADE_STATE.BATTLE;


                        ref var mapNodeEvIDComponent = ref nodeEvIDsStash.Get(ent);


                        // CODE GOES HERE


                        break;
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}