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

        public float doubleClickTime = 0.2f;
        private float lastClickTime;

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
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickTime)
            {
                if (count > 0)
                {
                    bool res = craftController.QuickPlaceResourceInSlot(this);
                    if (res)
                    {
                        count--;
                    }
                }
            }
            else
            {
                if (count > 0)
                {
                    count--;
                    craftController.PickResourceFromStorage(this);
                }
            }

            UpdateDisplay();
            lastClickTime = Time.time;
        }
    }
}