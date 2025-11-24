using Domain.Map;
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
    public class LabMonsterSlotMono : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public SpriteRenderer sprite_renderer;
        public TMP_Text name_text;
        public bool is_occupied;
        public bool isExpeditionSlot = false;
        public Button deleteButton;

        public MonsterData currentMonsterData { get; private set; }
        public MonsterTooltipTrigger tooltipTrigger { get; private set; }

        private GameObject dragCopy;
        private bool isDragging = false;
        private LabReferences labRef;

        public void Initialize()
        {
            labRef = LabReferences.Instance();
            is_occupied = false;
            sprite_renderer.sprite = null;
            name_text.text = null;

            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(OnDeleteButtonClicked);
                deleteButton.gameObject.SetActive(!isExpeditionSlot);
            }

            tooltipTrigger = GetComponent<MonsterTooltipTrigger>();
            if (tooltipTrigger == null)
            {
                tooltipTrigger = gameObject.AddComponent<MonsterTooltipTrigger>();
            }

            AddColliderIfNeeded();
        }

        private void OnDeleteButtonClicked()
        {
            if (is_occupied && currentMonsterData != null)
            {
                if (LabReferences.Instance().tutorialController.IsTutorialActive()) { return; }

                Debug.Log($"Delete button clicked for monster: {currentMonsterData.m_MonsterName}");
                var monsterToDelete = currentMonsterData;
                ClearSlot();

                if (labRef.monstersController != null)
                {
                    labRef.monstersController.DeleteMonster(monsterToDelete);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !isDragging)
            {
                HandleSingleClick();
            }
        }

        private void HandleSingleClick()
        {
            if (!is_occupied || currentMonsterData == null) return;

            LabMonsterExpeditionSlotMono freeExpeditionSlot = FindFirstFreeExpeditionSlot();
            if (freeExpeditionSlot != null)
            {
                if (labRef.expeditionController != null && labRef.expeditionController.IsMonsterInExpedition(currentMonsterData))
                {
                    Debug.Log($"Cannot add monster '{currentMonsterData.m_MonsterName}' to expedition - already added!");
                    return;
                }

                freeExpeditionSlot.OccupySelf(currentMonsterData);
                labRef.expeditionController?.OnExpeditionSlotsChanged();
                labRef.monstersController?.OnPreparationScreenChanged();
            }
        }

        private LabMonsterExpeditionSlotMono FindFirstFreeExpeditionSlot()
        {
            if (labRef.expeditionController == null || labRef.expeditionController.expeditionSlots == null)
                return null;

            foreach (var slot in labRef.expeditionController.expeditionSlots)
            {
                if (slot != null && !slot.is_occupied)
                    return slot;
            }
            return null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!is_occupied) return;
            CreateDragCopy();
            isDragging = true;
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
            StartCoroutine(HandleDragEndWithDelay(eventData));
        }

        private System.Collections.IEnumerator HandleDragEndWithDelay(PointerEventData eventData)
        {
            yield return null;

            LabMonsterExpeditionSlotMono expeditionTarget = FindExpeditionTargetUnderMouse();
            if (expeditionTarget != null && !expeditionTarget.is_occupied)
            {
                if (labRef.expeditionController != null && !labRef.expeditionController.IsMonsterInExpedition(currentMonsterData))
                {
                    expeditionTarget.OccupySelf(currentMonsterData);
                    labRef.monstersController?.OnPreparationScreenChanged();
                }
            }
            else
            {
                LabMonsterSlotMono targetSlot = FindTargetSlotUnderMouse();
                if (targetSlot != null && targetSlot != this && !targetSlot.is_occupied)
                {
                    MoveMonsterToSlot(targetSlot);
                }
            }

            Destroy(dragCopy);
            dragCopy = null;
            isDragging = false;
        }

        private LabMonsterExpeditionSlotMono FindExpeditionTargetUnderMouse()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
            foreach (Collider2D collider in colliders)
            {
                LabMonsterExpeditionSlotMono slot = collider.GetComponent<LabMonsterExpeditionSlotMono>();
                if (slot != null) return slot;
            }
            return null;
        }

        private LabMonsterSlotMono FindTargetSlotUnderMouse()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
            foreach (Collider2D collider in colliders)
            {
                LabMonsterSlotMono slot = collider.GetComponent<LabMonsterSlotMono>();
                if (slot != null && slot != this) return slot;
            }
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
        }

        private void MoveMonsterToSlot(LabMonsterSlotMono targetSlot)
        {
            MonsterData monsterToMove = currentMonsterData;
            ClearSlot();
            targetSlot.OccupySelf(monsterToMove);
            labRef.monstersController?.SaveCurrentState();
            labRef.previewController?.OnMonsterOrderChanged();
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
                if (deleteButton != null) deleteButton.gameObject.SetActive(!isExpeditionSlot);
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
            if (deleteButton != null) deleteButton.gameObject.SetActive(false);
            if (tooltipTrigger != null) tooltipTrigger.enabled = false;
            labRef.craftController?.UpdateCreateButtonState();
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
            if (dragCopy != null) Destroy(dragCopy);
        }
    }
}