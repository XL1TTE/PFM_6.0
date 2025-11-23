using Domain.Map;
using UnityEngine;
using System.Collections;

namespace Project
{
    public class LabCraftSlotMono : MonoBehaviour
    {
        [Header("Refs")]
        public SpriteRenderer slotImage;
        public SpriteRenderer partIcon;

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

        public void Initialize()
        {
            pairCraftSlot = null;

            craftController = LabReferences.Instance().craftController;

            partIcon.color = Color.clear;
            slotImage.sprite = normalSprite;

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
                }
                else
                {
                    slotImage.sprite = normalSprite;
                }

                yield return new WaitForSeconds(blinkInterval);
            }

            slotImage.sprite = normalSprite;
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
                        return true;
                    }
                }
            }
            StopHighlightBlink();
            return false;
        }

        public void UpdatePartData(BodyPartData resource)
        {
            containedPart = resource;
            partIcon.sprite = resource.icon;
            partIcon.color = Color.white;
            slotImage.sprite = normalSprite;
            StopHighlightBlink();
        }

        public void HighlightSlot()
        {
            slotImage.sprite = highlightedSprite;
        }

        public void ClearSlot()
        {
            containedPart = null;
            partIcon.color = Color.clear;
            slotImage.sprite = normalSprite;
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
        }
    }
}