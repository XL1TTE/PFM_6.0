using TMPro;
using UnityEngine;

namespace UI.Widgets
{
    public class PlateWithText : MonoBehaviour{
        public static PlateWithText Instance {get; private set;}

        [SerializeField] private TextMeshProUGUI TMP;

        void Awake()
        {
            if(Instance == null){
                Instance = this;
            }

            DisableSelf();
        }
        
        public void ChangeText(string text) => TMP.text = text; 

        public void Show(string message) {
            TMP.text = message; 
            EnableSelf();
        }
        
        public void Hide(){
            DisableSelf();
        }

        private void EnableSelf() => gameObject.SetActive(true);
        private void DisableSelf() => gameObject.SetActive(false);
    }

}
