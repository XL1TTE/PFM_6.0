using TMPro;
using UnityEngine;

namespace Domain.UI.Widgets
{
    public sealed class InformationBoardWidget: MonoBehaviour{
        [SerializeField] private TextMeshProUGUI TMP;

        public void ChangeText(string text) => TMP.text = text;

        public void Show(string message)
        {
            TMP.text = message;
            EnableSelf();
        }

        public void Hide()
        {
            DisableSelf();
        }

        private void EnableSelf() => gameObject.SetActive(true);
        private void DisableSelf() => gameObject.SetActive(false);
    }

}
