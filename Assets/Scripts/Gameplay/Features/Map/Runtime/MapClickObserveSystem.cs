
using Domain.CursorDetection.Components;
using Domain.Map.Components;
using Domain.Map.Events;
using Domain.Map.Requests;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Game;
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

        private Filter choicesUnderCursorFilter;

        private Filter nodesUnderCursorFilter;
        private Filter nodesWithIdComponent;

        public Stash<MapNodeIdComponent> nodeIdStash;
        private Stash<TagMapNodeBinder> nodeBinderStash;
        private Stash<TagMapNodeChoosable> nodeChoosableStash;
        private Stash<MapTextEvChoiceComponent> mapChoicesStash;

        private Stash<MapNodeEventId> nodeEvIDsStash;
        private Stash<MapNodeEventType> nodeTypesStash;

        //private Request<MapTextEventEnterRequest> req_draw_text_ui;
        private Event<MapNodeClickedEvent> ev_clicked_node;
        private Request<MapTextEventExecuteRequest> req_exe_choice;
        private Request<MapUpdateProgress> req_update_progress;

        //private Stash<ButtonTag> stash_btnTag;

        public void OnAwake()
        {
            World = ECS_Main_Map.m_mapWorld;
            choicesUnderCursorFilter = World.Filter
                .With<MapTextEvChoiceComponent>()
                .With<UnderCursorComponent>()
                .Build();

            nodesUnderCursorFilter = World.Filter
                .With<TagMapNodeBinder>()
                .With<UnderCursorComponent>()
                .Build();

            nodesWithIdComponent = World.Filter
                .With<MapNodeIdComponent>()
                .Build();

            nodeIdStash = World.GetStash<MapNodeIdComponent>();
            nodeBinderStash = World.GetStash<TagMapNodeBinder>();

            nodeChoosableStash = World.GetStash<TagMapNodeChoosable>();

            mapChoicesStash = World.GetStash<MapTextEvChoiceComponent>();

            nodeEvIDsStash = World.GetStash<MapNodeEventId>();
            nodeTypesStash = World.GetStash<MapNodeEventType>();

            ev_clicked_node = World.GetEvent<MapNodeClickedEvent>();

            req_exe_choice = World.GetRequest<MapTextEventExecuteRequest>();
            req_update_progress = World.GetRequest<MapUpdateProgress>();
            //req_draw_text_ui = World.GetRequest<MapTextEventEnterRequest>();

            //evt_btnClicked = World.GetEvent<ButtonClickedEvent>();
            //stash_btnTag = World.GetStash<ButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (nodesUnderCursorFilter.IsEmpty()
                    && choicesUnderCursorFilter.IsEmpty()) { return; }


                // IF NEED BE, THIS SYSTEM CAN BE REBUILD TO WORK WITH STATEMACHINE WORLD... if only it worked in general
                if (!choicesUnderCursorFilter.IsEmpty() && SM.IsStateActive<MapTextEvState>(out var state_text))
                {
                    var clickedChoice = choicesUnderCursorFilter.First();
                    var actualChoiceId = mapChoicesStash.Get(clickedChoice).count_id;

                    req_exe_choice.Publish(new MapTextEventExecuteRequest
                    {
                        choice_id = actualChoiceId,
                    });
                    return;
                }
                if (!nodesUnderCursorFilter.IsEmpty() && SM.IsStateActive<MapDefaultState>(out var state_def))
                {
                    var clickedNodeBinder = nodesUnderCursorFilter.First();

                    // DOING GODS WORK HERE
                    // wtf does that supposed to mean????

                    //var actualNodeId = clickedNodeBinder.GetComponent<TagMapNodeBinder>().node_id;
                    var actualNodeId = nodeBinderStash.Get(clickedNodeBinder).node_id;

                    foreach (Entity ent in nodesWithIdComponent)
                    {
                        ref var mapNodeIdComponent = ref nodeIdStash.Get(ent);

                        if (mapNodeIdComponent.node_id == actualNodeId && nodeChoosableStash.Has(ent))
                        {
                            ref var mapNodeEvTypeComponent = ref nodeTypesStash.Get(ent);
                            ref var mapNodeEvIDComponent = ref nodeEvIDsStash.Get(ent);


                            Debug.Log($"CLICKED ON ENTITY WITH NODE ID {actualNodeId}");
                            Debug.Log($"NODE EVENT TYPE IS {mapNodeEvTypeComponent.event_type}");
                            Debug.Log($"NODE EVENT ID IS {mapNodeEvIDComponent.event_id}");

                            req_update_progress.Publish(new MapUpdateProgress
                            {
                                end_node = actualNodeId
                            });

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
        }

        public void Dispose()
        {

        }
    }

}
