using Domain.Map;
using UnityEngine;

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

        public BODYPART_TYPE requiredType;

        private BodyPartData containedPart;
        private LabMonsterCraftController craftController;
        private LabBodyPartStorageMono originalStorageSlot;

        private LabCraftSlotMono pairCraftSlot;

        //private bool isOccupied;

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

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    if (eventData.button == PointerEventData.InputButton.Left)
        //    {
        //        if (craftController.isHoldingResource)
        //        {
        //            // ≈сли держим ресурс - попытатьс€ поместить его в слот
        //            TryPlaceResource(craftController.heldPartData);
        //        }
        //        else if (containedPart != null)
        //        {
        //            // ≈сли в слоте есть ресурс - вз€ть его обратно
        //            craftController.PickResourceFromCrafting(this);
        //            ClearSlot();
        //        }
        //    }
        //}

        public bool TryPlaceResource(BodyPartData resource)
        {
            if (resource.type == requiredType)
            {
                if (!IsOccupied())
                {
                    UpdatePartData(resource);

                    craftController.PlaceResourceInSlot(this);
                    return true;
                }
                else if (pairCraftSlot != null)
                {
                    if (!pairCraftSlot.IsOccupied())
                    {
                        craftController.MoveResourceFromToSlot(this, pairCraftSlot);

                        craftController.PlaceResourceInSlot(this);
                        UpdatePartData(resource);

                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdatePartData(BodyPartData resource)
        {
            containedPart = resource;
            partIcon.sprite = resource.icon;
            partIcon.color = Color.white;
            slotImage.sprite = normalSprite;
        }

        public void HighlightSlot()
        {
            slotImage.sprite = highlightedSprite;
        }

        public void ClearSlot()
        {
            //isOccupied = false;

            containedPart = null;
            partIcon.color = Color.clear;
            slotImage.sprite = normalSprite;
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
                // ≈сли в слоте есть ресурс - вз€ть его обратно
                craftController.PickResourceFromCrafting(this);
                ClearSlot();
            }
        }
    }
}
