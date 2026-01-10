using Domain.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private BodyPartData partData;
        private TooltipController tooltipController;
        private bool isHovering = false;

        public void Initialize(BodyPartData data)
        {
            partData = data;
            tooltipController = LabReferences.Instance().tooltipController;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isHovering && partData != null)
            {
                isHovering = true;
                tooltipController.ShowTooltip(partData, transform.position);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isHovering)
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
    }
}