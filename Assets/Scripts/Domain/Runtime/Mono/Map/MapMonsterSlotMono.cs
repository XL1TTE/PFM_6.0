using Domain.Map;
using Domain.Monster.Mono;
using Domain.TurnSystem.Components;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project
{
    public class MapMonsterSlotMono : MonoBehaviour
    {
        public SpriteRenderer sprite_renderer;
        public Image sprite_renderer_IMAGE;
        public TMP_Text name_text;
        public bool is_occupied;

        // Ссылка на контроллер тултипа для карты
        [Header("Tooltip Settings")]
        public MonsterTooltipController mapTooltipController;

        public MonsterData currentMonsterData { get; private set; }
        public MonsterTooltipTrigger tooltipTrigger { get; private set; }

        private MapMonstersController mapMonstersController;
        private bool isInitialized = false;

        public void Initialize()
        {
            if (isInitialized) return;

            mapMonstersController = FindObjectOfType<MapMonstersController>();
            is_occupied = false;
            sprite_renderer.sprite = null;
            sprite_renderer_IMAGE.sprite = null;
            sprite_renderer_IMAGE.color = Color.clear;
            name_text.text = null;

            // Инициализация MonsterTooltipTrigger
            InitializeTooltipTrigger();

            AddColliderIfNeeded();

            isInitialized = true;
        }

        private void InitializeTooltipTrigger()
        {
            // Получаем или добавляем компонент MonsterTooltipTrigger
            tooltipTrigger = GetComponent<MonsterTooltipTrigger>();
            if (tooltipTrigger == null)
            {
                tooltipTrigger = gameObject.AddComponent<MonsterTooltipTrigger>();
                Debug.Log("Added MonsterTooltipTrigger to map slot");
            }

            // Если контроллер тултипа не установлен в инспекторе, ищем на сцене
            if (mapTooltipController == null)
            {
                mapTooltipController = FindObjectOfType<MonsterTooltipController>();
                Debug.Log($"Found tooltip controller on scene: {mapTooltipController != null}");
            }
        }

        public void OccupySelf(MonsterData data)
        {
            if (data != null)
            {
                currentMonsterData = data;
                if (DataBase.TryFindRecordByID(data.Head_id, out var e_record))
                {
                    if (DataBase.TryGetRecord<AvatarUI>(e_record, out var e_icon))
                    {
                        sprite_renderer.sprite = e_icon.m_Value;
                        sprite_renderer.color = Color.white;
                        sprite_renderer_IMAGE.sprite = e_icon.m_Value;
                        sprite_renderer_IMAGE.color = Color.white;
                    }
                }
                name_text.text = !string.IsNullOrEmpty(data.m_MonsterName) ? data.m_MonsterName : "Unnamed Monster";

                // Инициализируем tooltip trigger с данными монстра
                if (tooltipTrigger != null && mapTooltipController != null)
                {
                    tooltipTrigger.InitializeForMap(data, mapTooltipController);
                    tooltipTrigger.enabled = true;
                    Debug.Log($"Tooltip initialized for monster: {data.m_MonsterName}");
                }
                else
                {
                    Debug.LogWarning($"Tooltip not initialized: trigger={tooltipTrigger != null}, controller={mapTooltipController != null}");
                }

                is_occupied = true;
            }
        }

        public void ClearSlot()
        {
            is_occupied = false;
            currentMonsterData = null;
            sprite_renderer.sprite = null;
            sprite_renderer_IMAGE.sprite = null;
            sprite_renderer_IMAGE.color = Color.clear;
            name_text.text = "";

            if (tooltipTrigger != null)
                tooltipTrigger.enabled = false;
        }

        private void AddColliderIfNeeded()
        {
            var collider = GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(1.5f, 1.5f);
                collider.isTrigger = true;
            }
        }

        void Start()
        {
            Initialize();
        }

        void OnDisable()
        {
            if (tooltipTrigger != null)
            {
                tooltipTrigger.OnDisable();
            }
        }
    }
}