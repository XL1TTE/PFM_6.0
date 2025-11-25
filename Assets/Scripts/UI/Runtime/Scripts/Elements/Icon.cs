namespace UI.Elements
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class Icon : IUIElement, IPoolElement
    {
        [SerializeField] private Image m_Icon;
        [SerializeField] private LayoutElement m_ElementLayout;

        public void Free(IUIElementPool<Icon> pool)
        {
            pool?.FreeElement(this);
        }

        public Icon SetIcon(Sprite a_icon)
        {
            m_Icon.sprite = a_icon;
            return this;
        }

        public Icon MinSize(int size)
        {
            m_ElementLayout.minWidth = size;
            m_ElementLayout.minHeight = size;
            return this;
        }

        internal override void Reset()
        {
            base.Reset();
            MinSize(0);
        }

    }
}
