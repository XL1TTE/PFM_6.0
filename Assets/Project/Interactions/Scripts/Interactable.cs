using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Project.ObjectInteractions
{
    [RequireComponent(typeof(EventTrigger), typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour
    {
        private EventTrigger m_eventTrigger;

        private UnityAction<BaseEventData> m_onPointerClick;
        private UnityAction<BaseEventData> m_onPointerEnter;
        private UnityAction<BaseEventData> m_onPointerExit;
        private UnityAction<BaseEventData> m_onPointerDown;
        private UnityAction<BaseEventData> m_onPointerUp;
        private UnityAction<BaseEventData> m_onBeginDrag;
        private UnityAction<BaseEventData> m_onDrag;
        private UnityAction<BaseEventData> m_onEndDrag;
        
        protected virtual void Awake()
        {
            m_eventTrigger = GetComponent<EventTrigger>();
            InitializeEventSystem();
        }

        EventTrigger.Entry pointeClickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
        EventTrigger.Entry dragEntry = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };

        private void InitializeEventSystem()
        {
            pointeClickEntry.callback.AddListener((eventData) => m_onPointerClick?.Invoke(eventData));
            pointerEnterEntry.callback.AddListener((eventData) => m_onPointerEnter?.Invoke(eventData));
            pointerExitEntry.callback.AddListener((eventData) => m_onPointerExit?.Invoke(eventData));
            pointerDownEntry.callback.AddListener((eventData) => m_onPointerDown?.Invoke(eventData));
            pointerUpEntry.callback.AddListener((eventData) => m_onPointerUp?.Invoke(eventData));
            beginDragEntry.callback.AddListener((eventData) => m_onBeginDrag?.Invoke(eventData));
            dragEntry.callback.AddListener((eventData) => m_onDrag?.Invoke(eventData));
            endDragEntry.callback.AddListener((eventData) => m_onEndDrag?.Invoke(eventData));

            m_eventTrigger.triggers = new List<EventTrigger.Entry>
            {
                pointeClickEntry,
                pointerEnterEntry,
                pointerExitEntry,
                pointerDownEntry,
                pointerUpEntry,
                beginDragEntry,
                dragEntry,
                endDragEntry
            };
        }

        #region Добавление обработчиков
        public void AddPointerClickListener(UnityAction<BaseEventData> action) => m_onPointerClick += action;
        public void AddPointerEnterListener(UnityAction<BaseEventData> action) => m_onPointerEnter += action;
        public void AddPointerExitListener(UnityAction<BaseEventData> action) => m_onPointerExit += action;
        public void AddPointerDownListener(UnityAction<BaseEventData> action) => m_onPointerDown += action;
        public void AddPointerUpListener(UnityAction<BaseEventData> action) => m_onPointerUp += action;
        public void AddBeginDragListener(UnityAction<BaseEventData> action) => m_onBeginDrag += action;
        public void AddDragListener(UnityAction<BaseEventData> action) => m_onDrag += action;
        public void AddEndDragListener(UnityAction<BaseEventData> action) => m_onEndDrag += action;
        #endregion

        #region Удаление обработчиков
        public void RemovePointerClickListener(UnityAction<BaseEventData> action) => m_onPointerClick -= action;
        public void RemovePointerEnterListener(UnityAction<BaseEventData> action) => m_onPointerEnter -= action;
        public void RemovePointerExitListener(UnityAction<BaseEventData> action) => m_onPointerExit -= action;
        public void RemovePointerDownListener(UnityAction<BaseEventData> action) => m_onPointerDown -= action;
        public void RemovePointerUpListener(UnityAction<BaseEventData> action) => m_onPointerUp -= action;
        public void RemoveBeginDragListener(UnityAction<BaseEventData> action) => m_onBeginDrag -= action;
        public void RemoveDragListener(UnityAction<BaseEventData> action) => m_onDrag -= action;
        public void RemoveEndDragListener(UnityAction<BaseEventData> action) => m_onEndDrag -= action;
        #endregion

        private void OnDestroy()
        {
            m_onPointerClick = null;
            m_onPointerEnter = null;
            m_onPointerExit = null;
            m_onPointerDown = null;
            m_onPointerUp = null;
            m_onBeginDrag = null;
            m_onDrag = null;
            m_onEndDrag = null;
        }
    }
}
