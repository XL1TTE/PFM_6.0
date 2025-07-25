using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.ObjectInteractions
{
    public class SmoothDrag: IDraggable{
        
        [Header("Drag Settings")]
        [SerializeField][Range(0.1f, 20f)] private float dampingSpeed = 10f;
        [SerializeField] private float zDepth = 5f;

        private Vector3 mousePosition;

        protected override void DragBeginHandler(BaseEventData eventData)
        {
            isDragging = true;

            PointerEventData MouseEventData = eventData as PointerEventData;
            mousePosition = g_MainCamera.ScreenToWorldPoint(
                new Vector3(MouseEventData.position.x, MouseEventData.position.y, -zDepth));
        }

        protected override void DragHandler(BaseEventData eventData)
        {
            PointerEventData MouseEventData = eventData as PointerEventData;
            mousePosition = g_MainCamera.ScreenToWorldPoint(
                new Vector3(MouseEventData.position.x, MouseEventData.position.y, -zDepth));
        }

        protected override void DragReleaseHandler(BaseEventData eventData)
        {
            transform.position = mousePosition;
            isDragging = false;
        }

        private void FixedUpdate()
        {
            if (isDragging)
            {
                transform.position = Vector3.Lerp(transform.position, mousePosition, dampingSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
