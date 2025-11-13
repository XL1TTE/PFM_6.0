namespace UI.ToolTip
{
    using UnityEngine;

    public abstract class IBlockLayout : MonoBehaviour
    {
        public void InsertElement(ILineElement a_element)
            => a_element.transform.SetParent(this.transform);
    }
}
