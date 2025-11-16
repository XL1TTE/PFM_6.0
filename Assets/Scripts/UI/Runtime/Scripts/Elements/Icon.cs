namespace UI.Elements
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class Icon : IUIElement, IPoolElement
    {
        [SerializeField] private Image m_Icon;

        public event Action OnFree;

        public void AttachPool(IUIElementPool<Icon> pool)
        {
            throw new NotImplementedException();
        }

        public void Free(IUIElementPool<Icon> pool)
        {
            pool?.FreeElement(this);
        }

        public void Free()
        {
            OnFree?.Invoke();
        }

        public Icon SetIcon(Sprite a_icon)
        {
            m_Icon.sprite = a_icon;
            return this;
        }

    }
}
