namespace UI.ToolTip
{
    using UnityEngine;

    public abstract class ILineElement : MonoBehaviour
    {
        internal virtual void SetLayout(Transform a_layout)
            => this.gameObject.transform.SetParent(a_layout, false);
    }
}
