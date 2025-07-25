using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Project.ObjectInteractions
{
    [RequireComponent(typeof(Interactable))]
    public abstract class IDraggable : MonoBehaviour
    {
  
        void Awake()
        {
            m_Collider = gameObject.GetComponent<Collider2D>();

            m_interactable = GetComponent<Interactable>();
        }

        protected void OnEnable()
        {
            g_MainCamera = Camera.main;
        }


        private Interactable m_interactable;

        [HideInInspector] public bool isDragging;
        [HideInInspector] public Collider2D m_Collider;
        [HideInInspector] protected Camera g_MainCamera;
        
        public event UnityAction NotifyDragBegin;
        public event UnityAction NotifyDragEnd;

        void OnDestroy()
        {
            // Remove all listeners
            DisableDragBehaviour();
        }
        
        public void EnableDragBehaviour(){
            m_interactable.AddBeginDragListener(OnDragBegin);
            m_interactable.AddDragListener(OnDrag);
            m_interactable.AddEndDragListener(OnDragEnd);
        }
        
        public void DisableDragBehaviour(){
            m_interactable.RemoveBeginDragListener(OnDragBegin);
            m_interactable.RemoveDragListener(OnDrag);
            m_interactable.RemoveEndDragListener(OnDragEnd);
        }
        
        private void OnDragBegin(BaseEventData eventData){
            DragBeginHandler(eventData);
            NotifyDragBegin?.Invoke();
        }
        private void OnDrag(BaseEventData eventData){
            DragHandler(eventData);
        }
        private void OnDragEnd(BaseEventData eventData){
            DragReleaseHandler(eventData);
            NotifyDragEnd?.Invoke();
        }
        
           
        #region DragHandlers
            
            protected virtual void DragBeginHandler(BaseEventData eventData){
                isDragging = true;
            }
            protected virtual void DragHandler(BaseEventData eventData){
                
            }
            protected virtual void DragReleaseHandler(BaseEventData eventData){
                isDragging = false;
            }
            
        #endregion
    }
}
