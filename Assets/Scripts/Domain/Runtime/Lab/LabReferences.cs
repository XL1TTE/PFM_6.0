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

        [Header("Cameras")]
        public Transform mainCameraContainer;
        public Camera mainCamera;

        [Header("Heads")]
        public GameObject headsGridRef;
        public GameObject headCraftSlotRef;

        [Header("Torsos")]
        public GameObject torsosGridRef;
        public GameObject torsoCraftSlotRef;

        [Header("Arms")]
        public GameObject armsGridRef;
        public GameObject armLCraftSlotRef;
        public GameObject armRCraftSlotRef;

        [Header("Legs")]
        public GameObject legsGridRef;
        public GameObject legLCraftSlotRef;
        public GameObject legRCraftSlotRef;

        [Header("Containers")]
        public GameObject monsterSlotsContainer;
        public GameObject craftSlotsContainer;
        public GameObject heldPartContainer;
        public GameObject monsterPreviewContainer;

        [Header("Controllers")]
        public LabMonsterCraftController craftController;
        public LabBodyPartHeldMono heldPartMono;
        public LabMonstersController monstersController;
        public LabUIController uiController;
        public ExpeditionController expeditionController;
        public PreparationMonsterPreviewController previewController;
        public LabMonstersCounter monstersCounter;
        public LabSceneChanger sceneChanger;
        public TutorialController tutorialController;

        [Header("Tooltips")]
        public MonsterTooltipController monsterTooltipController;
        public MonsterTooltipController preparationMonsterTooltipController;
        public TooltipController tooltipController;

        [Header("UI Elements")]
        public GameObject createButton;
        public MonsterNamingPanel namingPanel;

        // Методы для безопасного доступа
        public Transform GetMonsterPreviewContainer() => monsterPreviewContainer?.transform;
        public Transform GetMonsterSlotsContainer() => monsterSlotsContainer?.transform;
        public Transform GetCraftSlotsContainer() => craftSlotsContainer?.transform;

        public Transform GetHeadsContainer() => headsGridRef?.transform;
        public Transform GetTorsosContainer() => torsosGridRef?.transform;
        public Transform GetArmsContainer() => armsGridRef?.transform;
        public Transform GetLegsContainer() => legsGridRef?.transform;
    }
}