namespace UI.Elements
{
    using System;
    using TMPro;
    using UnityEngine;

    [Serializable]
    public class Text : IUIElement, IPoolElement
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        public event Action OnFree;

        public Text SetText(string a_text)
        {
            m_Text.text = a_text;
            return this;
        }

        public Text SetColor(Color color)
        {
            m_Text.color = color;
            return this;
        }

        public Text AlignCenter()
        {
            m_Text.alignment = TextAlignmentOptions.Center;
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

        internal override void Reset()
        {
            base.Reset();
            Free();
        }

        public void Free()
        {
            OnFree?.Invoke();
        }
    }
}
