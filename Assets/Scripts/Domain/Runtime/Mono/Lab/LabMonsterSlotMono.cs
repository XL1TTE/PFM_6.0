using Domain.Monster.Mono;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Project
{
    public class LabMonsterSlotMono : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public SpriteRenderer sprite_renderer;
        public TMP_Text name_text;
        public bool is_occupied;

        public Button deleteButton;

        public MonsterData currentMonsterData { get; private set; }
        public MonsterTooltipTrigger tooltipTrigger { get; private set; }

        // Drag & Drop
        private GameObject dragCopy;
        private bool isDragging = false;
        private LabMonstersController monstersController;

        public void Initialize()
        {
            is_occupied = false;
            sprite_renderer.sprite = null;
            name_text.text = null;

            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(OnDeleteButtonClicked);
                deleteButton.gameObject.SetActive(false);
            }

            tooltipTrigger = GetComponent<MonsterTooltipTrigger>();
            if (tooltipTrigger == null)
            {
                tooltipTrigger = gameObject.AddComponent<MonsterTooltipTrigger>();
            }

            monstersController = FindObjectOfType<LabMonstersController>();
            AddColliderIfNeeded();
        }

        private void OnDeleteButtonClicked()
        {
            if (is_occupied && currentMonsterData != null)
            {
                Debug.Log($"Delete button clicked for monster: {currentMonsterData.m_MonsterName}");

                var monsterToDelete = currentMonsterData;
                ClearSlot();

                if (monstersController != null)
                {
                    monstersController.DeleteMonster(monsterToDelete);
                }
                else
                {
                    Debug.LogError("MonstersController not found!");
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!is_occupied) return;

            CreateDragCopy();
            isDragging = true;
            Debug.Log($"Started dragging monster: {currentMonsterData.m_MonsterName}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging || dragCopy == null) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            mousePosition.z = 0;
            dragCopy.transform.position = mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Debug.Log("End drag detected");

            LabMonsterSlotMono targetSlot = FindTargetSlotUnderMouse();

            if (targetSlot != null && targetSlot != this && !targetSlot.is_occupied)
            {
                MoveMonsterToSlot(targetSlot);
                Debug.Log($"Successfully moved to slot: {targetSlot.name}");
            }
            else
            {
                Debug.Log("No valid target slot found");
            }

            Destroy(dragCopy);
            dragCopy = null;
            isDragging = false;
        }

        private LabMonsterSlotMono FindTargetSlotUnderMouse()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
            foreach (Collider2D collider in colliders)
            {
                LabMonsterSlotMono slot = collider.GetComponent<LabMonsterSlotMono>();
                if (slot != null && slot != this)
                {
                    Debug.Log($"Found slot: {slot.name}, occupied: {slot.is_occupied}");
                    return slot;
                }
            }

            Debug.Log("No slot found under mouse");
            return null;
        }

        private void CreateDragCopy()
        {
            dragCopy = new GameObject($"{name}_DragCopy");
            dragCopy.transform.SetParent(transform.root);

            SpriteRenderer copySprite = dragCopy.AddComponent<SpriteRenderer>();
            copySprite.sprite = sprite_renderer.sprite;
            copySprite.sortingOrder = 1000;
            copySprite.color = new Color(1f, 1f, 1f, 0.8f);

            dragCopy.transform.localScale = transform.localScale;
            dragCopy.transform.position = transform.position;

            Debug.Log("Drag copy created");
        }

        private void MoveMonsterToSlot(LabMonsterSlotMono targetSlot)
        {
            MonsterData monsterToMove = currentMonsterData;

            ClearSlot();
            targetSlot.OccupySelf(monsterToMove);

            if (monstersController != null)
            {
                monstersController.SaveCurrentState();

                var previewController = FindObjectOfType<PreparationMonsterPreviewController>();
                if (previewController != null)
                {
                    previewController.OnMonsterOrderChanged();
                    Debug.Log($"Preview updated after moving monster to {targetSlot.name}");
                }
                else
                {
                    Debug.LogWarning("PreparationMonsterPreviewController not found!");
                }
            }

            Debug.Log($"Moved monster '{monsterToMove.m_MonsterName}' from {name} to {targetSlot.name}");
        }

        public void OccupySelf(MonsterData data)
        {
            if (data != null)
            {
                currentMonsterData = data;

                if (DataBase.TryFindRecordByID(data.Head_id, out var e_record))
                {
                    if (DataBase.TryGetRecord<IconUI>(e_record, out var e_icon))
                    {
                        sprite_renderer.sprite = e_icon.m_Value;
                        sprite_renderer.color = Color.white;
                    }
                }

                name_text.text = !string.IsNullOrEmpty(data.m_MonsterName) ? data.m_MonsterName : "Unnamed Monster";

                if (deleteButton != null)
                {
                    deleteButton.gameObject.SetActive(true);
                }

                if (tooltipTrigger != null)
                {
                    tooltipTrigger.Initialize(data);
                    tooltipTrigger.enabled = true;
                }

                is_occupied = true;

            }
        }

        public void ClearSlot()
        {
            is_occupied = false;
            currentMonsterData = null;
            sprite_renderer.sprite = null;
            name_text.text = "";

            if (deleteButton != null)
            {
                deleteButton.gameObject.SetActive(false);
            }

            if (tooltipTrigger != null)
                tooltipTrigger.enabled = false;

            var craftController = FindObjectOfType<LabMonsterCraftController>();
            if (craftController != null)
            {
                craftController.UpdateCreateButtonState();
            }

            Debug.Log($"Slot {name} cleared");
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

        void OnDestroy()
        {
            if (dragCopy != null)
                Destroy(dragCopy);
        }
    }
}