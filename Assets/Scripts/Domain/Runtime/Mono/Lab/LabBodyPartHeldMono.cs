using Domain.Map;
using UnityEngine;

namespace Project
{
    public class LabBodyPartHeldMono : MonoBehaviour
    {
        private Transform craftingSlotsContainer;
        private LabMonsterCraftController craftController;
        public SpriteRenderer part_sprite;

        private new BoxCollider collider;

        public float zOffset = 0;

        public void Initialize()
        {
            var labRef = LabReferences.Instance();
            craftingSlotsContainer = labRef.GetCraftSlotsContainer();
            craftController = labRef.craftController;
            this.transform.SetParent(labRef.heldPartContainer.transform);

            collider = GetComponent<BoxCollider>();
        }

        public void SetPart(BodyPartData part)
        {
            part_sprite.sprite = part.icon;
            part_sprite.color = Color.white;
        }

        public void ShowSelf(bool val)
        {
            if (val)
            {
                part_sprite.color = Color.white;
                collider.enabled = true;
            }
            else
            {
                part_sprite.color = Color.clear;
                collider.enabled = false;
            }
        }

        public void Update()
        {
            FollowMouse();

            if (Input.GetMouseButtonUp(0))
            {
                bool placedSuccessfully = false;

                foreach (Transform child in craftingSlotsContainer)
                {
                    BoxCollider2D otherCollider = child.gameObject.GetComponent<BoxCollider2D>();
                    bool doesIntersect = collider.bounds.Intersects(otherCollider.bounds);
                    if (doesIntersect)
                    {
                        LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                        if (slot != null)
                        {
                            if (craftController.isHoldingResource)
                            {
                                placedSuccessfully = slot.TryPlaceResource(craftController.heldPartData);
                                if (placedSuccessfully) break;
                            }
                        }
                    }
                }
                if (!placedSuccessfully)
                {
                    craftController.ReturnHeldResource();
                }
            }
        }

        public void FollowMouse()
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = zOffset;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            transform.position = mouseWorldPosition;
        }
    }
}