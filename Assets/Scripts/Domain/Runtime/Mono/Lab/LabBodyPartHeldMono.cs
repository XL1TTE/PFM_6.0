using Domain.Map;
using UnityEngine;

namespace Project
{
    public class LabBodyPartHeldMono : MonoBehaviour//, IDragHandler
    {
        private Transform craftingSlotsContainer;
        private LabMonsterCraftController craftController;
        public SpriteRenderer part_sprite;

        private BoxCollider collider;

        public float zOffset = 0; // Adjust this value to control the object's distance from the camera

        public void Initialize()
        {
            craftingSlotsContainer = LabReferences.Instance().craftSlotsContainer.transform;
            craftController = LabReferences.Instance().craftController;
            this.transform.SetParent(LabReferences.Instance().heldPartContainer.transform);

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

            if (Input.GetMouseButtonUp(0)) {

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
                                // ≈сли держим ресурс - попытатьс€ поместить его в слот
                                slot.TryPlaceResource(craftController.heldPartData);
                                break;
                            }
                        }
                    }

                }

                craftController.ReturnHeldResource();
            }
        }

        //public void OnMouseUp()
        //{
        //    Debug.Log("mouse up");

        //    foreach (Transform child in craftingSlotsContainer)
        //    {

        //        BoxCollider2D otherCollider = child.GetComponent<BoxCollider2D>();
        //        bool doesIntersect = collider.bounds.Intersects(otherCollider.bounds);
        //        if (doesIntersect)
        //        {
        //            LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
        //            if (slot != null && slot.GetContainedResource() == null)
        //            {
        //                if (craftController.isHoldingResource)
        //                {
        //                    // ≈сли держим ресурс - попытатьс€ поместить его в слот
        //                    slot.TryPlaceResource(craftController.heldPartData);
        //                    break;
        //                }
        //            }
        //        }

        //    }

        //    craftController.ReturnHeldResource();
        //}

        public void FollowMouse()
        {

            // Get the mouse position in screen coordinates
            Vector3 mouseScreenPosition = Input.mousePosition;

            // Set the Z-coordinate for the world point.
            // This determines how far into the scene the object will be placed.
            // A higher zOffset moves the object further from the camera.
            mouseScreenPosition.z = zOffset;

            // Convert the screen position to world coordinates
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            // Set the object's position to the calculated world position
            transform.position = mouseWorldPosition;
        }
    }
}
