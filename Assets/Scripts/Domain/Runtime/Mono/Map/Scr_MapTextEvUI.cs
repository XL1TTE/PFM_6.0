using Domain.Map;
using Domain.Map.Components;
using Domain.Map.Mono;
using Domain.Map.Providers;
using Game;
using Scellecs.Morpeh;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Domain
{
    public class Scr_MapTextEvUI : MonoBehaviour
    {
        public Transform ptr_whole;
        public SpriteRenderer ptr_sprite;
        public TextMeshPro ptr_message;
        public Transform ptr_choices;


        public float dist_between_options = 30;
    }
}
