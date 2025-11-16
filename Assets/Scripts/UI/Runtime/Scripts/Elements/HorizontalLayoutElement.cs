namespace UI.Elements
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class HorizontalLayoutElement : IUIElement, IPoolElement
    {
        [SerializeField] private HorizontalLayoutGroup m_HorizontalLayout;

        private List<IUIElement> m_Childrens = new(4);

        public event Action OnFree;

        public HorizontalLayoutElement AlignCenter()
        {
            m_HorizontalLayout.childAlignment = TextAnchor.MiddleCenter;
            return this;
        }

        public HorizontalLayoutElement AlignStart()
        {
            m_HorizontalLayout.childAlignment = TextAnchor.MiddleLeft;
            return this;
        }
        public HorizontalLayoutElement AlignEnd()
        {
            m_HorizontalLayout.childAlignment = TextAnchor.MiddleRight;
            return this;
        }

        public HorizontalLayoutElement Insert(IUIElement a_element)
        {
            a_element.SetLayout(m_HorizontalLayout.transform);
            a_element.transform.SetAsLastSibling();

            m_Childrens.Add(a_element);
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

        public IEnumerable<IUIElement> GetChildrens() => m_Childrens;

        public void Free()
        {
            OnFree?.Invoke();
        }
    }
}
