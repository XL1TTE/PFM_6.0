namespace UI.ToolTip
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class Icon : ILineElement
    {
        [SerializeField] private Image m_Icon;
        public void SetIcon(Sprite a_icon) => m_Icon.sprite = a_icon;
    }
}
