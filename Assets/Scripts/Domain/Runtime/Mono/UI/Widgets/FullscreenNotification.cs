using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class FullscreenNotification : MonoBehaviour
    {

        private static FullscreenNotification m_instance;
        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            HideSelf();
        }

        [SerializeField] private TextMeshProUGUI m_primaryMessage;
        [SerializeField] private TextMeshProUGUI m_tipMessage;
        [SerializeField] private Image m_background;


        #region Public API
        public static bool IsInstantiated() => m_instance != null;
        public static void ShowMessage(string message, string tip = "")
        {
            m_instance.m_primaryMessage.text = message;
            m_instance.m_tipMessage.text = tip;
            m_instance.ShowSelf();
        }
        public static void HideMessage() => m_instance.HideSelf();
        public static void DestroySelf() => Destroy(m_instance.gameObject);
        #endregion

        private static void Reset()
        {
            m_instance.m_primaryMessage.text = "";
            m_instance.m_tipMessage.text = "";
        }
        private void ShowSelf() => gameObject.SetActive(true);
        private void HideSelf() => gameObject.SetActive(false);


        public static void SetBgColor(Color a_color) => m_instance.m_background.color = a_color;
    }

}
