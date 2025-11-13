namespace UI.ToolTip
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using UI.ToolTip;
    using UnityEngine;
    using UnityEngine.UI;

    public class ToolTip : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup m_Layout;
        [SerializeField] private CanvasGroup m_CanvasGroup;

        private Tween m_AlphaAnim;

        [SerializeField] private Vector2 m_CursorOffset = new Vector2(10, -10);
        [SerializeField] private float m_ScreenMargin = 20f;

        [SerializeField] private RectTransform m_TooltipRect;
        private void Awake()
        {
            m_TooltipRect = GetComponent<RectTransform>();
            m_CanvasGroup.alpha = 0;
        }


        public void Show(ToolTipLines a_config)
        {
            ResetState();

            PrepareState(a_config);

            m_AlphaAnim?.Kill(true);
            m_AlphaAnim = m_CanvasGroup.DOFade(1, 0.25f);

            UpdatePosition();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void PrepareState(ToolTipLines a_config)
        {
            gameObject.SetActive(true);

            if (a_config.m_Lines.Count <= 0)
            {
                gameObject.SetActive(false);
            }

            foreach (var line in a_config.m_Lines)
            {
                line.SetExternalLayout(m_Layout.transform);
                line.Show();
            }
        }

        private void ResetState()
        {
            for (int i = 0; i < m_Layout.transform.childCount; ++i)
            {
                Destroy(m_Layout.transform.GetChild(i).gameObject);
            }
        }

        public void Hide()
        {
            m_AlphaAnim?.Kill(true);
            m_AlphaAnim = m_CanvasGroup.DOFade(0, 0.25f);

            gameObject.SetActive(false);
        }

        private void Reset()
        {

        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            if (m_TooltipRect == null)
            {
                Debug.LogError("RectTransform is missing!");
                return;
            }

            Vector2 mousePos = Input.mousePosition;
            Vector2 size = m_TooltipRect.sizeDelta;

            bool rightSide = mousePos.x + size.x + m_ScreenMargin < Screen.width;
            bool topSide = mousePos.y + size.y + m_ScreenMargin < Screen.height;

            m_TooltipRect.pivot = new Vector2(
                rightSide ? 0 : 1,
                topSide ? 0 : 1
            );

            float posX = rightSide ?
                Mathf.Clamp(mousePos.x + m_CursorOffset.x, m_ScreenMargin, Screen.width - size.x - m_ScreenMargin) :
                Mathf.Clamp(mousePos.x - m_CursorOffset.x, m_ScreenMargin, Screen.width - m_ScreenMargin);

            float posY = topSide ?
                Mathf.Clamp(mousePos.y - m_CursorOffset.y, m_ScreenMargin, Screen.height - size.y - m_ScreenMargin) :
                Mathf.Clamp(mousePos.y + m_CursorOffset.y, m_ScreenMargin, Screen.height - m_ScreenMargin);

            m_TooltipRect.position = new Vector2(posX, posY);
        }
    }

}
