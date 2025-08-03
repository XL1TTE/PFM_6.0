using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class NotifyMessageController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private Image _background;

        void Awake()
        {
            HideSelf();
        }

        public void ShowMessage(string message){
            _message.text = message;
            ShowSelf();
        }
        
        public void HideMessage(){
            HideSelf();
        }
        
        public void DestroySelf() => Destroy(gameObject);
    
        private void ShowSelf() => gameObject.SetActive(true);
        private void HideSelf() => gameObject.SetActive(false);
    }
}
