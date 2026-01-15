using Domain.Map;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project
{
    public class LabCraftSlotMono : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Refs")]
        public SpriteRenderer slotImage;
        public SpriteRenderer partIcon;

        public Image slotImageIMAGE;
        public Image partIconIMAGE;

        [Header("Sprites")]
        public Sprite normalSprite;
        public Sprite highlightedSprite;

        [Header("Blink Settings")]
        public float blinkInterval = 0.5f;

        private bool isDragInProgress = false;
        private Vector3 mouseDownPosition;
        private const float DRAG_THRESHOLD = 5f;

        public BODYPART_TYPE requiredType;

        private BodyPartData containedPart;
        private LabMonsterCraftController craftController;
        private LabBodyPartStorageMono originalStorageSlot;

        private LabCraftSlotMono pairCraftSlot;

        private bool isBlinking = false;
        private Coroutine blinkCoroutine;

        private TooltipController tooltipController;
        private bool isHovering = false;

        public void Initialize()
        {
            pairCraftSlot = null;
            craftController = LabReferences.Instance().craftController;
            tooltipController = LabReferences.Instance().tooltipController;

            partIcon.color = Color.clear;
            partIconIMAGE.color = Color.clear;

            slotImage.sprite = normalSprite;
            slotImageIMAGE.sprite = normalSprite;

            var labRef = LabReferences.Instance();
            switch (requiredType)
            {
                case BODYPART_TYPE.ARM:
                    if (labRef.armLCraftSlotRef == gameObject)
                    {
                        pairCraftSlot = labRef.armRCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    else
                    {
                        pairCraftSlot = labRef.armLCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    break;
                case BODYPART_TYPE.LEG:
                    if (labRef.legLCraftSlotRef == gameObject)
                    {
                        pairCraftSlot = labRef.legRCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    else
                    {
                        pairCraftSlot = labRef.legLCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    break;
            }
        }

        // Обработчик наведения мыши
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsOccupied() && tooltipController != null)
            {
                isHovering = true;
                tooltipController.ShowTooltip(containedPart, transform.position);
            }
        }

        // Обработчик выхода мыши
        public void OnPointerExit(PointerEventData eventData)
        {
            if (isHovering && tooltipController != null)
            {
                isHovering = false;
                tooltipController.HideTooltip();
            }
        }

        private void OnDisable()
        {
            if (isHovering)
            {
                isHovering = false;
                if (tooltipController != null)
                    tooltipController.HideTooltip();
            }
        }

        public void OnMouseDown()
        {
            if (!IsOccupied()) return;

            mouseDownPosition = Input.mousePosition;
            isDragInProgress = false;
        }

        public void OnMouseDrag()
        {
            if (!IsOccupied()) return;

            if (!isDragInProgress && Vector3.Distance(Input.mousePosition, mouseDownPosition) > DRAG_THRESHOLD)
            {
                isDragInProgress = true;
                StartDragFromSlot();
            }
        }

        public void OnMouseUp()
        {
            if (!IsOccupied()) return;

            if (!isDragInProgress)
            {
                ReturnToInventory();
            }
            isDragInProgress = false;
        }

        private void StartDragFromSlot()
        {
            craftController.PickResourceFromCrafting(this);
            ClearSlot();
            Debug.Log($"Started drag from craft slot with {containedPart?.partName}");
        }

        private void ReturnToInventory()
        {
            if (IsOccupied() && originalStorageSlot != null)
            {
                originalStorageSlot.ReturnResource();
                ClearSlot();

                craftController.SubstrPreviewPoint();
                craftController.UpdateCreateButtonState();

                AudioManager.Instance?.PlaySound(AudioManager.putSound);
                Debug.Log($"Returned {containedPart?.partName} to inventory via single click");
            }
        }

        public void StartHighlightBlink()
        {
            if (!isBlinking && !IsOccupied())
            {
                isBlinking = true;
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(BlinkCoroutine());
            }
        }

        public void StopHighlightBlink()
        {
            if (isBlinking)
            {
                isBlinking = false;
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                    blinkCoroutine = null;
                }
                slotImage.sprite = normalSprite;
                slotImageIMAGE.sprite = normalSprite;
            }
        }

        private IEnumerator BlinkCoroutine()
        {
            bool showHighlighted = false;

            while (isBlinking)
            {
                showHighlighted = !showHighlighted;

                if (showHighlighted)
                {
                    slotImage.sprite = highlightedSprite;
                    slotImageIMAGE.sprite = highlightedSprite;
                }
                else
                {
                    slotImage.sprite = normalSprite;
                    slotImageIMAGE.sprite = normalSprite;
                }

                yield return new WaitForSeconds(blinkInterval);
            }

            slotImage.sprite = normalSprite;
            slotImageIMAGE.sprite = normalSprite;
        }

        private void OnMouseEnter()
        {
            if (!IsOccupied() && craftController.isHoldingResource)
            {
                BodyPartData heldPart = craftController.heldPartData;
                if (heldPart != null && heldPart.type == requiredType)
                {
                    StartHighlightBlink();
                }
            }
        }

        public bool TryPlaceResource(BodyPartData resource)
        {
            if (resource.type == requiredType)
            {
                if (!IsOccupied())
                {
                    UpdatePartData(resource);
                    craftController.PlaceResourceInSlot(this);
                    StopHighlightBlink();

                    AudioManager.Instance?.PlaySound(AudioManager.putSound);
                    return true;
                }
                else if (pairCraftSlot != null)
                {
                    if (!pairCraftSlot.IsOccupied())
                    {
                        craftController.MoveResourceFromToSlot(this, pairCraftSlot);
                        craftController.PlaceResourceInSlot(this);
                        UpdatePartData(resource);
                        StopHighlightBlink();

                        AudioManager.Instance?.PlaySound(AudioManager.putSound);
                        return true;
                    }
                }
            }
            StopHighlightBlink();

            AudioManager.Instance?.PlaySound(AudioManager.buttonErrorSound);
            return false;
        }

        public void UpdatePartData(BodyPartData resource)
        {
            containedPart = resource;

            partIcon.sprite = resource.icon;
            partIconIMAGE.sprite = resource.icon;

            partIcon.color = Color.white;
            partIconIMAGE.color = Color.white;

            slotImage.sprite = normalSprite;
            slotImageIMAGE.sprite = normalSprite;

            StopHighlightBlink();
        }

        public void HighlightSlot()
        {
            slotImage.sprite = highlightedSprite;
            slotImageIMAGE.sprite = highlightedSprite;
        }

        public void ClearSlot()
        {
            // Скрываем тултип если он показывался для этого слота
            if (isHovering && tooltipController != null)
            {
                isHovering = false;
                tooltipController.HideTooltip();
            }

            containedPart = null;

            partIcon.color = Color.clear;
            partIconIMAGE.color = Color.clear;

            slotImage.sprite = normalSprite;
            slotImageIMAGE.sprite = normalSprite;

            StopHighlightBlink();
        }

        public void SetOriginalSlot(LabBodyPartStorageMono origin)
        {
            originalStorageSlot = origin;
        }
        public LabBodyPartStorageMono GetOriginalSlot()
        {
            return originalStorageSlot;
        }
        public BodyPartData GetContainedResource()
        {
            return containedPart;
        }
        public bool IsOccupied()
        {
            return (containedPart != null);
        }

        private void OnDestroy()
        {
            StopHighlightBlink();

            if (isHovering && tooltipController != null)
            {
                tooltipController.HideTooltip();
            }
        }
    }
}