namespace UI.Elements
{
    using System;
    using Game;
    using UnityEngine;

    public abstract class IUIElement : MonoBehaviour, IPoolElement
    {
        private Vector3 m_Scale;

        public event Action OnFree;

        void Awake()
        {
            m_Scale = this.gameObject.transform.localScale;
        }

        internal virtual void SetLayout(Transform a_layout)
            => this.gameObject.transform.SetParent(a_layout, false);

        public virtual void Show() => gameObject.SetActive(true);

        internal virtual void Reset()
        {
            this.gameObject.transform.localScale = m_Scale;
        }

        public void Free()
        {
            OnFree?.Invoke();
        }
    }
}
