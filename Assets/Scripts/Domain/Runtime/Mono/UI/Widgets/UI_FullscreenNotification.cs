using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class UI_FullscreenNotification: MonoBehaviour{
        [SerializeField] private TextMeshProUGUI _primaryMessage;
        [SerializeField] private TextMeshProUGUI _tipMessage;
        [SerializeField] private Image _background;

        void Awake()
        {
            HideSelf();
        }

        public void ShowMessage(string message, string tip = "")
        {
            _primaryMessage.text = message;
            _tipMessage.text = tip;
            ShowSelf();
        }

        public void HideMessage()
        {
            HideSelf();
        }

        private void Reset()
        {
            _primaryMessage.text = "";
            _tipMessage.text = "";
        }

        public void DestroySelf() => Destroy(gameObject);

        private void ShowSelf() => gameObject.SetActive(true);
        private void HideSelf() => gameObject.SetActive(false);
    }

}
