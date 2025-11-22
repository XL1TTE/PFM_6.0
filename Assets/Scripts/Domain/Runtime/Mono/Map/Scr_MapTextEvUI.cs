using Gameplay.MapEvents.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain
{
    public class Scr_MapTextEvUI : MonoBehaviour
    {
        public Transform ptr_whole;
        public Image ptr_sprite;
        public TextMeshProUGUI ptr_message;
        public Transform ptr_choices;
        public Transform ptr_continue_button;

        [HideInInspector] public MapTextEventHandlerSystem textHandler;

        [ContextMenu("click")]
        public void ClickedContinue()
        {
            textHandler.UnDrawTextUI();
        }
    }
}
