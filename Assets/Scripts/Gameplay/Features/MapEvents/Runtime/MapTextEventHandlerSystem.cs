using Domain;
using Domain.BattleField.Requests;
using Domain.DragAndDrop.Requests;
using Domain.Map.Events;
using Domain.Map.Mono;
using Domain.Map.Requests;
using Domain.MapEvents.Requests;
using NUnit;
using Persistence.DB;
using Scellecs.Morpeh;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Gameplay.MapEvents.Systems
{
    public sealed class MapTextEventHandlerSystem : MonoBehaviour, ISystem
    {
        private Sprite bg_sprite = null;
        private string string_message = null;
        private Dictionary<string, MapChoiceWrapper> choices = null;

        private bool flag_ui_is_shown = false;

        private Request<MapTextEventEnterRequest> req_draw_text_ui;
        private Request<MapTextEventExecuteRequest> req_exe_choice;

        private Request<GiveGoldRequest> req_give_gold;
        private Request<TakeGoldRequest> req_take_gold;


        private GameObject prefabedMainUI;

        //private Filter all_events_text;

        //private Stash<MapNodeIdComponent> nodeIdStash;
        //private Stash<MapNodeEventId> nodeEventIdStash;


        private GameObject textEvMainPrefab;
        //private GameObject textEvChoicePrefab;


        public World World { get; set; }

        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }

        public void OnAwake()
        {
            //all_events_text = DataBase.Filter.With<MapEvTextTag>().Build();

            req_draw_text_ui = World.GetRequest<MapTextEventEnterRequest>();
            req_exe_choice = World.GetRequest<MapTextEventExecuteRequest>();

            req_give_gold = World.GetRequest<GiveGoldRequest>();
            req_take_gold = World.GetRequest<TakeGoldRequest>();

            //nodeIdStash = World.GetStash<MapNodeIdComponent>();
            //nodeEventIdStash = World.GetStash<MapNodeEventId>();

            textEvMainPrefab = Resources.Load<GameObject>("Map/Prefabs/MapTextEvUIPrefab");
            //textEvChoicePrefab = Resources.Load<GameObject>("Map/Prefabs/MapTextEvChoicePrefab");
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_draw_text_ui.Consume())
            {
                //if (StateMachineWorld.IsStateActive<MapDefaultState>(out var state))
                if (!flag_ui_is_shown)
                {
                    //    StateMachineWorld.ExitState<MapDefaultState>();
                    //    StateMachineWorld.EnterState<MapTextEvState>();
                    flag_ui_is_shown = true;

                    Debug.Log("WE ARE IN DRAWING VISUALS FOR TEXT EVENTS");

                    UpdateUIValues(req.event_id);
                    //DrawTextUI(req.event_id); 
                    DrawTextUI(); 

                    return;
                }
            }
            foreach (var req in req_exe_choice.Consume())
            {
                //if (StateMachineWorld.IsStateActive<MapTextEvState>(out var state))
                if (flag_ui_is_shown)
                {
                    flag_ui_is_shown = false;
                    //StateMachineWorld.ExitState<MapTextEvState>();
                    //StateMachineWorld.EnterState<MapDefaultState>();

                    Debug.Log("WE ARE IN SOLVING A CHOSEN CHOICE FUR TEXT EVENTS");

                    UnDrawTextUI();

                    var tmp_count = 0;
                    foreach (var choice in choices)
                    {
                        if (tmp_count == req.choice_id)
                        {
                            //choice.Value.

                            switch (choice.Value.type)
                            {
                                case CHOICE_SCRIPT_TYPE.GIVE_GOLD:
                                    req_give_gold.Publish((GiveGoldRequest)choice.Value.request);
                                    break;
                                case CHOICE_SCRIPT_TYPE.TAKE_GOLD:
                                    req_take_gold.Publish((TakeGoldRequest)choice.Value.request);
                                    break;
                            }

                            //ev_clicked_node.NextFrame(new MapNodeClickedEvent
                            //{
                            //    node_entity = ent,
                            //});

                            return;
                        }
                        tmp_count++;
                    }
                }
            }


            ////StateMachineWorld.EnterState<MapDefaultState>();
            //StateMachineWorld.ExitState<MapDefaultState>();
            //StateMachineWorld.EnterState<MapTextEvState>();

            //if (StateMachineWorld.IsStateActive<MapDefaultState>(out var state))
            //{
            //    // CAN do something with "state"
            //}


            //req_give_gold.Publish(new GiveGoldRequest
            //{
            //    
            //});
            //req_take_gold.Publish(new TakeGoldRequest
            //{
            //
            //});
            return;
        }

        //private void DrawTextUI(string event_id)
        private void DrawTextUI()
        {

            prefabedMainUI = Instantiate(textEvMainPrefab, new Vector3( 0 , 0 , 0 ), Quaternion.identity);

            prefabedMainUI.GetComponent<Scr_MapTextEvUI>().VisualiseUI(bg_sprite, string_message, choices);

        }
        private void UnDrawTextUI()
        {

            prefabedMainUI.GetComponent<Scr_MapTextEvUI>().DeVisualiseUI();

        }

        private void UpdateUIValues(string event_id)
        {
            if (DataBase.TryFindRecordByID(event_id, out var found_record))
            {
                if (found_record == null)
                {
                    throw new System.Exception($"Event in DB: {event_id} was not found.");
                }

                // Get BG path
                if (DataBase.TryGetRecord<MapEvTextBGComponent>(found_record, out var res_bg))
                {
                    Debug.Log(res_bg.bg_sprite_path);
                    bg_sprite = Resources.Load<Sprite>(res_bg.bg_sprite_path);
                }

                // Get main base text message
                if (DataBase.TryGetRecord<MapEvTextMessageComponent>(found_record, out var res_main_text))
                {
                    Debug.Log(res_main_text.string_message);
                    string_message = res_main_text.string_message;
                }

                // Get all of the choices available
                if (DataBase.TryGetRecord<MapEvTextChoicesComponent>(found_record, out var res_choices))
                {
                    Debug.Log(res_choices.choices);
                    choices = res_choices.choices;
                }
            }
            else
            {
                throw new System.Exception($"Event in DB: {event_id} was not found. SOMETHING IS VERY WRONG!");
            }
        }
    }

}