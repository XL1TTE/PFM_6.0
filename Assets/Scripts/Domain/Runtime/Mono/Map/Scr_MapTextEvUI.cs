using Domain.Extentions;
using Domain.Map.Components;
using Domain.Map.Providers;
using Scellecs.Morpeh;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Domain
{
    public class Scr_MapTextEvUI : MonoBehaviour
    {
        public World World { get; set; }

        public GameObject textEvChoicePrefab;

        public Stash<MapTextEvChoiceComponent> stash;
        public Filter filter;

        public Transform ptr_whole;
        public SpriteRenderer ptr_sprite;
        public TextMeshPro ptr_message;
        public Transform ptr_choices;


        public float dist_between_options = 30;
        private float choices_x_pos;
        private float choices_y_pos;

        private void Start()
        {
            Camera mainCamera = Camera.main;

            Vector3 cameraPosition = mainCamera.transform.position;
            Vector3 positionWithoutZ = new Vector3(cameraPosition.x, cameraPosition.y, 0f);

            ptr_whole.transform.position = positionWithoutZ;
        }

        //public void OnAwake()
        //{
        //    //World = World.Default;

        //    filter = World.Filter.With<MapTextEvChoiceComponent>().Build();

        //    this.stash = this.World.GetStash<MapTextEvChoiceComponent>();
        //}

        public void VisualiseUI(Sprite bg_sprite, string string_message, Dictionary<string, IRequestData> choices)
        {
            World = World.Default;
            filter = World.Filter.With<MapTextEvChoiceComponent>().Build();

            this.stash = this.World.GetStash<MapTextEvChoiceComponent>();


            ptr_sprite.sprite = bg_sprite;
            ptr_message.text = string_message;

            //choices_x_pos = ptr_choices.position.x;
            //choices_y_pos = ptr_choices.position.y;

            choices_x_pos = 0;
            choices_y_pos = 0;

            float tmp_whole_dist = (choices.Count - 1) * dist_between_options;
            float tmp_start_y = (choices_y_pos + tmp_whole_dist / 2) ;
            int count = 0;

            foreach (var choice in choices)
            {
                var tmp_curr_y = tmp_start_y - dist_between_options*count;

                var prefabedChoice = Instantiate(textEvChoicePrefab, new Vector3(choices_x_pos, tmp_curr_y, 0), Quaternion.identity);
                prefabedChoice.GetComponentInChildren<TextMeshPro>().text = choice.Key;

                prefabedChoice.transform.SetParent(ptr_choices, false);

                if (prefabedChoice.TryGetComponent<MapTextEvChoiceProvider>(out var t_choice))
                {
                    t_choice.GetData().count_id = count;
                }

                count++;
            }

        }

        public void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }


        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
