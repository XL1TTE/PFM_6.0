using Domain.Map;
using TMPro;
using UnityEngine;

namespace Project
{
    public class LabBodyPartStorageMono : MonoBehaviour
    {
        public SpriteRenderer part_sprite;
        public int count;
        public TMP_Text count_text;

        private bool isDragInProgress = false;
        private Vector3 mouseDownPosition;
        private const float DRAG_THRESHOLD = 5f;

        public string part_id;
        public BODYPART_TYPE part_type;

        public LabMonsterCraftController craftController;
        public BodyPartData bodyPartData;
        private TooltipTrigger tooltipTrigger;

        public void Initialize(BodyPartData data, int c)
        {
            bodyPartData = data;
            count = c;
            part_type = data.type;

            craftController = LabReferences.Instance().craftController;

            tooltipTrigger = GetComponent<TooltipTrigger>();
            if (tooltipTrigger == null)
            {
                tooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
            }
            tooltipTrigger.Initialize(data);

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (count > 0)
            {
                part_sprite.color = Color.white;
                count_text.text = count.ToString();
            }
            else
            {
                part_sprite.color = Color.gray;
                count_text.text = "0";
            }
        }

        public void ReturnResource()
        {
            count++;
            UpdateDisplay();
        }

        private void OnMouseEnter()
        {
            if (craftController != null && !craftController.isHoldingResource)
            {
                craftController.OnBodyPartHoverStart(this.bodyPartData);
            }
        }

        private void OnMouseExit()
        {
            if (craftController != null && !craftController.isHoldingResource)
            {
                craftController.OnBodyPartHoverEnd();
            }
        }

        public void OnMouseDown()
        {
            if (count <= 0) return;

            mouseDownPosition = Input.mousePosition;
            isDragInProgress = false;
        }

        public void OnMouseDrag()
        {
            if (!isDragInProgress && Vector3.Distance(Input.mousePosition, mouseDownPosition) > DRAG_THRESHOLD)
            {
                isDragInProgress = true;
                StartDrag();
            }
        }

        public void OnMouseUp()
        {
            if (!isDragInProgress)
            {
                HandleSingleClick();
            }
            isDragInProgress = false;
        }

        private void StartDrag()
        {
            if (count > 0)
            {
                count--;
                craftController.PickResourceFromStorage(this);
                UpdateDisplay();

                AudioManager.Instance?.PlaySound(AudioManager.whooshSound);

                Debug.Log($"Started drag with {bodyPartData.partName}");
            }
        }

        private void HandleSingleClick()
        {
            if (count > 0)
            {
                bool placedAutomatically = craftController.QuickPlaceResourceInSlot(this);

                if (placedAutomatically)
                {
                    count--;
                    Debug.Log($"Auto-placed {bodyPartData.partName} via single click");

                    AudioManager.Instance?.PlaySound(AudioManager.putSound);
                }
                else
                {
                    count--;
                    craftController.PickResourceFromStorage(this);
                    Debug.Log($"Picked {bodyPartData.partName} to hand via single click");

                    AudioManager.Instance?.PlaySound(AudioManager.whooshSound);
                }

                UpdateDisplay();
            }
            else
            {
                Debug.Log("No parts available to pick");
            }
        }
    }
}