using Domain.Map;
using UnityEngine;
using System.Collections;

namespace Project
{
    public class LabCraftSlotMono : MonoBehaviour//, IEndDragHandler//, IPointerClickHandler
    {
        [Header("Refs")]
        public SpriteRenderer slotImage;
        public SpriteRenderer partIcon;

        [Header("Sprites")]
        public Sprite normalSprite;
        public Sprite highlightedSprite;

        [Header("Blink Settings")]
        public float blinkInterval = 0.5f;

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

            switch (requiredType)
            {
                case BODYPART_TYPE.ARM:
                    if (LabReferences.Instance().armLCraftSlotRef == gameObject)
                    {
                        pairCraftSlot = LabReferences.Instance().armRCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    else
                    {
                        pairCraftSlot = LabReferences.Instance().armLCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    break;
                case BODYPART_TYPE.LEG:
                    if (LabReferences.Instance().legLCraftSlotRef == gameObject)
                    {
                        pairCraftSlot = LabReferences.Instance().legRCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    else
                    {
                        pairCraftSlot = LabReferences.Instance().legLCraftSlotRef.GetComponent<LabCraftSlotMono>();
                    }
                    break;
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

        public void OnMouseDown()
        {
            if (IsOccupied())
            {
                craftController.PickResourceFromCrafting(this);
                ClearSlot();
            }
        }

        private void OnDestroy()
        {
            StopHighlightBlink();
        }
    }
}
