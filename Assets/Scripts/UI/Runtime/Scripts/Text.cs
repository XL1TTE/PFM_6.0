namespace UI.ToolTip
{
    using System;
    using TMPro;
    using UnityEngine;

    [Serializable]
    public class Text : ILineElement
    {
        [SerializeField] private TextMeshProUGUI m_Text;
        public Text SetText(string a_text, Color a_color)
        {
            m_Text.text = a_text;
            m_Text.color = a_color;
            return this;
        }

        public Text FontSize(int a_size)
        {
            m_Text.fontSize = a_size;
            return this;
        }
        public Text Bold()
        {
            m_Text.fontWeight = FontWeight.Bold;
            return this;
        }
    }
}
