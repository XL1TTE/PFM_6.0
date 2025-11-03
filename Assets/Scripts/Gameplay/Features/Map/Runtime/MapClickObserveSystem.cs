
using Domain.CursorDetection.Components;
using Domain.Map.Components;
using Domain.Map.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


namespace Gameplay.Map.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapClickObserveSystem : ISystem
    {
        public World World { get; set; }

        private Filter nodesUnderCursorFilter;
        private Filter nodesWithIdComponent;

        public Stash<MapNodeIdComponent> nodeIdStash;
        private Stash<TagMapNodeBinder> nodeBinderStash;

        private Stash<MapNodeEventId> nodeEvIDsStash;
        private Stash<MapNodeEventType> nodeTypesStash;

        //private Request<MapTextEventEnterRequest> req_draw_text_ui;
        private Event<MapNodeClickedEvent> ev_clicked_node;

        //private Stash<ButtonTag> stash_btnTag;

        public void OnAwake()
        {
            nodesUnderCursorFilter = World.Filter
                .With<TagMapNodeBinder>()
                .With<UnderCursorComponent>()
                .Build();

            nodesWithIdComponent = World.Filter
                .With<MapNodeIdComponent>()
                .Build();

            nodeIdStash = World.GetStash<MapNodeIdComponent>();
            nodeBinderStash = World.GetStash<TagMapNodeBinder>();

            nodeEvIDsStash = World.GetStash<MapNodeEventId>();
            nodeTypesStash = World.GetStash<MapNodeEventType>();

            ev_clicked_node = World.GetEvent<MapNodeClickedEvent>();
            //req_draw_text_ui = World.GetRequest<MapTextEventEnterRequest>();

            //evt_btnClicked = World.GetEvent<ButtonClickedEvent>();
            //stash_btnTag = World.GetStash<ButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (nodesUnderCursorFilter.IsEmpty()) { return; }

                var clickedNodeBinder = nodesUnderCursorFilter.First();

                // DOING GODS WORK HERE
                // wtf does that supposed to mean????

                //var actualNodeId = clickedNodeBinder.GetComponent<TagMapNodeBinder>().node_id;
                var actualNodeId = nodeBinderStash.Get(clickedNodeBinder).node_id;

                foreach (Entity ent in nodesWithIdComponent)
                {
                    ref var mapNodeIdComponent = ref nodeIdStash.Get(ent);
                    if (mapNodeIdComponent.node_id == actualNodeId)
                    {
                        ref var mapNodeEvTypeComponent = ref nodeTypesStash.Get(ent);
                        ref var mapNodeEvIDComponent = ref nodeEvIDsStash.Get(ent);


                        Debug.Log($"CLICKED ON ENTITY WITH NODE ID {actualNodeId}");
                        Debug.Log($"NODE EVENT TYPE IS {mapNodeEvTypeComponent.event_type}");
                        Debug.Log($"NODE EVENT ID IS {mapNodeEvIDComponent.event_id}");


                        ev_clicked_node.NextFrame(new MapNodeClickedEvent
                        {
                            node_entity = ent,
                        });

                        //switch (mapNodeEvTypeComponent.event_type)
                        //{
                        //    case EVENT_TYPE.TEXT:
                        //        req_draw_text_ui.Publish(new MapTextEventEnterRequest
                        //        {
                        //            event_id = mapNodeEvIDComponent.event_id,
                        //        });
                        //        break;
                        //    case EVENT_TYPE.BATTLE:
                        //        break;
                        //    case EVENT_TYPE.LAB:
                        //        break;
                        //    case EVENT_TYPE.BOSS:
                        //        break;
                        //}

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
