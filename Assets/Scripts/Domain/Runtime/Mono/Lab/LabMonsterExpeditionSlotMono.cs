using Domain.Map;
using Domain.Monster.Mono;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Persistence.DB;
using Persistence.Components;
using UnityEngine.UI;

namespace Project
{
    public class LabMonsterExpeditionSlotMono : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public SpriteRenderer sprite_renderer;
        public Image sprite_renderer_IMAGE;
        public TMP_Text name_text;
        public bool is_occupied;

        public MonsterData currentMonsterData { get; private set; }
        public MonsterTooltipTrigger tooltipTrigger { get; private set; }

        private LabReferences labRef;
        private BoxCollider2D clickCollider;

        public void Initialize()
        {
            labRef = LabReferences.Instance();
            is_occupied = false;
            sprite_renderer.sprite = null;
            sprite_renderer_IMAGE.sprite = null;
            sprite_renderer_IMAGE.color = Color.clear;
            name_text.text = "Empty";

            tooltipTrigger = GetComponent<MonsterTooltipTrigger>();
            if (tooltipTrigger == null)
            {
                tooltipTrigger = gameObject.AddComponent<MonsterTooltipTrigger>();
            }

            AddColliderIfNeeded();
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Drop detected on expedition slot");

            LabMonsterSlotMono draggedMonster = eventData.pointerDrag?.GetComponent<LabMonsterSlotMono>();

            if (draggedMonster != null && draggedMonster.is_occupied && !this.is_occupied)
            {
                if (labRef.expeditionController != null && labRef.expeditionController.IsMonsterInExpedition(draggedMonster.currentMonsterData))
                {
                    Debug.Log($"Monster '{draggedMonster.currentMonsterData.m_MonsterName}' is already in expedition!");
                    return;
                }

                OccupySelf(draggedMonster.currentMonsterData);

                if (labRef.expeditionController != null)
                {
                    labRef.expeditionController.OnExpeditionSlotsChanged();
                }

                Debug.Log($"Monster '{currentMonsterData.m_MonsterName}' added to expedition slot {name}");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HandleClick();
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }
        }

        private void HandleClick()
        {
            if (is_occupied && currentMonsterData != null)
            {
                Debug.Log($"Clicked on expedition slot with monster: {currentMonsterData.m_MonsterName}");
                ClearSlot();

                if (labRef.expeditionController != null)
                {
                    labRef.expeditionController.OnExpeditionSlotsChanged();
                }

                Debug.Log($"Monster removed from expedition slot {name}");
            }
            else
            {
                Debug.Log($"Clicked on empty expedition slot: {name}");
            }
        }

        public void OccupySelf(MonsterData data)
        {
            if (data != null)
            {
                currentMonsterData = data;

                if (TryGetMonsterSprite(data, out Sprite monsterSprite))
                {
                    sprite_renderer.sprite = monsterSprite;
                    sprite_renderer.color = Color.white;
                    sprite_renderer_IMAGE.sprite = monsterSprite;
                    sprite_renderer_IMAGE.color = Color.white;
                }
                else
                {
                    Debug.LogWarning($"Failed to get sprite for monster: {data.m_MonsterName}");
                    sprite_renderer.sprite = null;
                    sprite_renderer_IMAGE.sprite = null;
                    sprite_renderer_IMAGE.color = Color.clear;
                }

                name_text.text = !string.IsNullOrEmpty(data.m_MonsterName) ? data.m_MonsterName : "Unnamed Monster";
                is_occupied = true;

                if (tooltipTrigger != null)
                {
                    tooltipTrigger.Initialize(data);
                    tooltipTrigger.enabled = true;
                }

                if (clickCollider != null)
                    clickCollider.enabled = true;

                LabReferences.Instance().tutorialController.ContinueSpecial();
            }
        }

        private bool TryGetMonsterSprite(MonsterData monsterData, out Sprite sprite)
        {
            sprite = null;

            if (monsterData != null)
            {
                if (DataBase.TryFindRecordByID(monsterData.Head_id, out var e_record))
                {
                    if (DataBase.TryGetRecord<IconUI>(e_record, out var e_icon))
                    {
                        sprite = e_icon.m_Value;
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearSlot()
        {
            is_occupied = false;
            currentMonsterData = null;
            sprite_renderer.sprite = null;
            sprite_renderer_IMAGE.sprite = null;
            sprite_renderer_IMAGE.color = Color.clear;
            name_text.text = "Empty";

            if (tooltipTrigger != null)
                tooltipTrigger.enabled = false;
        }

        public MonsterData GetMonsterAndClear()
        {
            MonsterData monster = currentMonsterData;
            ClearSlot();
            return monster;
        }

        private void AddColliderIfNeeded()
        {
            clickCollider = GetComponent<BoxCollider2D>();
            if (clickCollider == null)
            {
                clickCollider = gameObject.AddComponent<BoxCollider2D>();
                clickCollider.size = new Vector2(1.5f, 1.5f);
                clickCollider.isTrigger = true;
            }
            clickCollider.enabled = true;
        }

        void Start()
        {
            Initialize();
        }
    }
}