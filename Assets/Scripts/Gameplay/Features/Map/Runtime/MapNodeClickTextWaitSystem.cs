using Domain.Map.Events;
using Domain.Map.Requests;
using Scellecs.Morpeh;

namespace Gameplay.Map.Systems
{
    public sealed class MapNodeClickTextWaitSystem : ISystem 
    {
        public World World { get; set;}

        private Event<MapNodeClickedEvent> ev_clicked_node;
        private Request<MapTextEventEnterRequest> req_draw_text_ui;

        private Stash<MapNodeEventId> nodeEvIDsStash;
        private Stash<MapNodeEventType> nodeTypesStash;
        public void OnAwake()
        {
            ev_clicked_node = World.GetEvent<MapNodeClickedEvent>();
            req_draw_text_ui = World.GetRequest<MapTextEventEnterRequest>();

            nodeEvIDsStash = World.GetStash<MapNodeEventId>();
            nodeTypesStash = World.GetStash<MapNodeEventType>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in ev_clicked_node.publishedChanges)
            {
                if (evt.node_entity != null)
                {
                    var ent = evt.node_entity;

                    ref var mapNodeEvTypeComponent = ref nodeTypesStash.Get(ent);

                    if (mapNodeEvTypeComponent.event_type == EVENT_TYPE.TEXT)
                    {
                        ref var mapNodeEvIDComponent = ref nodeEvIDsStash.Get(ent);

                        req_draw_text_ui.Publish(new MapTextEventEnterRequest
                        {
                            event_id = mapNodeEvIDComponent.event_id,
                        });

                        return;
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}