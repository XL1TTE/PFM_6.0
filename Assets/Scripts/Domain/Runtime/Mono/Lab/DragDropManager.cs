using Domain.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project
{
    public class DragDropManager : MonoBehaviour
    {
        public static DragDropManager Instance { get; private set; }

        [Header("Settings")]
        public float dragAlpha = 0.6f;
        public bool snapBackOnFail = true;

        private LabReferences labRef;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            labRef = LabReferences.Instance();
        }

        public bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            return mousePos;
        }

        public void SetDragState(CanvasGroup canvasGroup, bool isDragging)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = isDragging ? dragAlpha : 1f;
                canvasGroup.blocksRaycasts = !isDragging;
            }
        }
    }
}