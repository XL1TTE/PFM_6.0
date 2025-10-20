using TMPro;
using UnityEngine;

namespace Domain.FloatingDamage
{
    public class FloatingDamageView : MonoBehaviour
    {

        private void Awake()
        {
            m_OriginalScale = gameObject.transform.localScale;
        }
        private Vector3 m_OriginalScale;

        [SerializeField] public TMP_Text Value;

        private Color m_textColor;
        public Color TextColor
        {
            get => m_textColor;
            set
            {
                m_textColor = value;
                this.Value.color = value;
            }
        }


        public void ResetView()
        {
            Value.text = "";
            m_textColor = Color.white;
            gameObject.transform.localScale = m_OriginalScale;
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
