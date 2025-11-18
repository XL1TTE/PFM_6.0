using Domain.Map;
using TMPro;
using UnityEngine;

namespace Project
{
    public class LabBodyPartStorageMono : MonoBehaviour//, IBeginDragHandler, IDragHandler//, IPointerClickHandler
    {
        public SpriteRenderer part_sprite;

        public int count;
        public TMP_Text count_text;


        public float doubleClickTime = 0.2f; // Time in seconds to consider a double click
        private float lastClickTime;


        // for viewing in unity editor
        public string part_id;
        public BODYPART_TYPE part_type;


        public LabMonsterCraftController craftController;

        public BodyPartData bodyPartData;

        public void Initialize(BodyPartData data, int c)
        {
            bodyPartData = data;
            count = c;
            part_type = data.type;

            craftController = LabReferences.Instance().craftController;

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

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    if (eventData.button == PointerEventData.InputButton.Left)
        //    {
        //        // ЛКМ - взять ресурс
        //        //if (count > 0 && !craftController.isHoldingResource)
        //        //{
        //        //    count--;
        //        //    UpdateDisplay();
        //        //    craftController.PickResourceFromStorage(this);
        //        //}

        //        // ЛКМ - взять ресурс
        //        if (count > 0)
        //        {
        //            count--;

        //            // если уже есть в руках, то сбросить старый, а потом взять новый
        //            if (craftController.isHoldingResource)
        //            {
        //                craftController.ReturnHeldResource();
        //            }

        //            UpdateDisplay();
        //            craftController.PickResourceFromStorage(this);
        //        }
        //    }
        //}

        public void ReturnResource()
        {
            count++;
            UpdateDisplay();
        }

        //void Update()
        //{
        //    // Check for left mouse button click
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        float timeSinceLastClick = Time.time - lastClickTime;

        //        if (timeSinceLastClick <= doubleClickTime)
        //        {
        //            Debug.Log("Double Click Detected!");
        //        }

        //        // Update the last click time for the next comparison
        //        lastClickTime = Time.time;
        //    }
        //}
        //public void OnDrag(PointerEventData eventData)
        //{

        //}
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

            // Update the last click time for the next comparison
            lastClickTime = Time.time;
        }
    }
}
