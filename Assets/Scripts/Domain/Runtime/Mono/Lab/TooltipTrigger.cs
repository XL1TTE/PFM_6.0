using Domain.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private BodyPartData partData;
        private TooltipController tooltipController;
        private bool isPointerOver;

        public void Initialize(BodyPartData data)
        {
            partData = data;
            tooltipController = LabReferences.Instance().tooltipController;
            isPointerOver = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipController.ShowTooltip(partData, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipController.HideTooltip();
        }

        private void OnDisable()
        {
            isPointerOver = false;
            if (tooltipController != null)
            {
                tooltipController.HideTooltip();
            }
        }
    }
}