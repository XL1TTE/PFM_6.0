namespace UI.ToolTip
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ToolTipLine : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup m_HorizontalLayout;

        [SerializeField] private Icon m_IconPrototype;
        [SerializeField] private Text m_TextPrototype;

        public Text WarmupText(string a_text, Color a_color)
        {

            var t_text = Instantiate(m_TextPrototype, m_HorizontalLayout.transform);

            t_text.SetText(a_text, a_color);
            t_text.gameObject.SetActive(false);

            return t_text;
        }
        public Icon WarmupIcon(Sprite a_icon)
        {
            var t_icon = Instantiate(m_IconPrototype, m_HorizontalLayout.transform);

            t_icon.SetIcon(a_icon);
            t_icon.gameObject.SetActive(false);

            return t_icon;
        }

        public ToolTipLine AlignCenter()
        {
            m_HorizontalLayout.childAlignment = TextAnchor.MiddleCenter;
            return this;
        }

        public ToolTipLine AlignStart()
        {
            m_HorizontalLayout.childAlignment = TextAnchor.MiddleLeft;
            return this;
        }
        public ToolTipLine AlignEnd()
        {
            m_HorizontalLayout.childAlignment = TextAnchor.MiddleRight;
            return this;
        }

        public ToolTipLine Insert(ILineElement a_element)
        {
            a_element.SetLayout(m_HorizontalLayout.transform);
            a_element.transform.SetAsLastSibling();
            return this;
        }

        public void SetExternalLayout(Transform a_layout) => transform.SetParent(a_layout);

        public void Show()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < m_HorizontalLayout.transform.childCount; ++i)
            {
                m_HorizontalLayout.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
