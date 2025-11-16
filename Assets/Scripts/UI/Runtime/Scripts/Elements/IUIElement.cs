namespace UI.Elements
{
    using UnityEngine;

    public abstract class IUIElement : MonoBehaviour
    {
        private Vector3 m_Scale;
        void Awake()
        {
            m_Scale = this.gameObject.transform.localScale;
        }

        internal virtual void SetLayout(Transform a_layout)
            => this.gameObject.transform.SetParent(a_layout, false);

        internal virtual void Reset()
        {
            this.gameObject.transform.localScale = m_Scale;
        }
    }
}
