using Project;
using UnityEngine;

namespace Domain.Map
{
    public class LabReferences : MonoBehaviour
    {
        private static LabReferences _instance;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static LabReferences Instance() => _instance;

        [Header("cams")]
        public Transform mainCameraContainer;
        public Camera mainCamera;

        [Header("heads")]
        public GameObject headsGridRef;
        public GameObject headCraftSlotRef;

        [Header("torsos")]
        public GameObject torsosGridRef;
        public GameObject torsoCraftSlotRef;

        [Header("arms")]
        public GameObject armsGridRef;
        public GameObject armLCraftSlotRef;
        public GameObject armRCraftSlotRef;

        [Header("legs")]
        public GameObject legsGridRef;
        public GameObject legLCraftSlotRef;
        public GameObject legRCraftSlotRef;

        [Header("Containers")]
        public GameObject monsterSlotsContainer;
        public GameObject craftSlotsContainer;
        public GameObject heldPartContainer;
        public GameObject monsterPreviewContainer;

        [Header("controllers")]
        public LabMonsterCraftController craftController;
        public LabBodyPartHeldMono heldPartMono;

        public MonsterTooltipController monsterTooltipController;
        public MonsterTooltipController preparationTooltipController;
        public TooltipController tooltipController;
    }
}