using Domain;
using Domain.Map;
using Domain.Map.Components;
using Domain.Map.Mono;
using Domain.Map.Providers;
using Domain.Map.Requests;
using Domain.MapEvents.Requests;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Game;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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


        public World World { get; set; }


        private GameObject textEvChoicePrefab;

        private Transform ptr_whole;
        private SpriteRenderer ptr_sprite;
        private TextMeshPro ptr_message;
        private Transform ptr_choices;


        private float dist_between_options = 30;
        private float choices_x_pos;
        private float choices_y_pos;

        private List<GameObject> prefabed_choices;
        private Camera mainCamera;
        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }
        public void OnAwake()
        {
            World = ECS_Main_Map.m_mapWorld;

            req_draw_text_ui = World.GetRequest<MapTextEventEnterRequest>();
            req_exe_choice = World.GetRequest<MapTextEventExecuteRequest>();

            req_give_gold = World.GetRequest<GiveGoldRequest>();
            req_take_gold = World.GetRequest<TakeGoldRequest>();


            prefabedMainUI = MapReferences.Instance().textEvUI;

            mainCamera = MapReferences.Instance().mainCamera;

            Scr_MapTextEvUI mainUI = prefabedMainUI.GetComponent<Scr_MapTextEvUI>();

            //textEvChoicePrefab = mainUI.textEvChoicePrefab;
            textEvChoicePrefab = Resources.Load<GameObject>("Map/Prefabs/MapTextEvChoicePrefab");

            ptr_whole = mainUI.ptr_whole;
            ptr_sprite = mainUI.ptr_sprite;
            ptr_message = mainUI.ptr_message;
            ptr_choices = mainUI.ptr_choices;
            dist_between_options = mainUI.dist_between_options;
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_draw_text_ui.Consume())
            {
                if (SM.IsStateActive<MapDefaultState>(out var state))
                //if (!flag_ui_is_shown)
                {
                    SM.ExitState<MapDefaultState>();
                    SM.EnterState<MapTextEvState>();
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
                if (SM.IsStateActive<MapTextEvState>(out var state))
                //if (flag_ui_is_shown)
                {
                    flag_ui_is_shown = false;
                    SM.ExitState<MapTextEvState>();
                    SM.EnterState<MapDefaultState>();

                    Debug.Log("WE ARE IN SOLVING A CHOSEN CHOICE FUR TEXT EVENTS");

                    UnDrawTextUI();



                    ref var crusadeState = ref DataStorage.GetRecordFromFile<Crusade, CrusadeState>();
                    crusadeState.crusade_state = CRUSADE_STATE.CHOOSING;



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
            return;
        }
        private void DrawTextUI()
        {
            prefabedMainUI.SetActive(true);



            Vector3 cameraPosition = mainCamera.transform.position;
            Vector3 positionWithoutZ = new Vector3(cameraPosition.x, cameraPosition.y, 0f);

            ptr_whole.transform.position = positionWithoutZ;




            prefabed_choices = new List<GameObject>(); // Initialize the list


            ptr_sprite.sprite = bg_sprite;
            ptr_message.text = string_message;

            //choices_x_pos = ptr_choices.position.x;
            //choices_y_pos = ptr_choices.position.y;

            choices_x_pos = 0;
            choices_y_pos = 0;

            float tmp_whole_dist = (choices.Count - 1) * dist_between_options;
            float tmp_start_y = (choices_y_pos + tmp_whole_dist / 2);
            int count = 0;

            foreach (var choice in choices)
            {
                var tmp_curr_y = tmp_start_y - dist_between_options * count;

                var prefabedChoice = Instantiate(textEvChoicePrefab, new Vector3(choices_x_pos, tmp_curr_y, 0), Quaternion.identity);
                prefabedChoice.GetComponentInChildren<TextMeshPro>().text = choice.Key;

                this.prefabed_choices.Add(prefabedChoice);

                prefabedChoice.transform.SetParent(ptr_choices, false);

                if (prefabedChoice.TryGetComponent<MapTextEvChoiceProvider>(out var t_choice))
                {
                    t_choice.GetData().count_id = count;
                }

                count++;
            }

        }
        private void UnDrawTextUI()
        {
            foreach (var choice in prefabed_choices)
            {
                Destroy(choice);
            }

            prefabed_choices.Clear();

            prefabedMainUI.SetActive(false);
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
                    //Debug.Log(res_bg.bg_sprite_path);
                    bg_sprite = Resources.Load<Sprite>(res_bg.bg_sprite_path);
                }

                // Get main base text message
                if (DataBase.TryGetRecord<MapEvTextMessageComponent>(found_record, out var res_main_text))
                {
                    //Debug.Log(res_main_text.string_message);
                    string_message = res_main_text.string_message;
                }

                // Get all of the choices available
                if (DataBase.TryGetRecord<MapEvTextChoicesComponent>(found_record, out var res_choices))
                {
                    //Debug.Log(res_choices.choices);
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