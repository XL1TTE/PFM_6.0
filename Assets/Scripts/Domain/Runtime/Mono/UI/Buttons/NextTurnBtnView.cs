using UnityEngine;

namespace Domain.UI.Mono
{
    public class NextTurnBtnView : MonoBehaviour
    {
        [SerializeField] private GameObject ViewObject;
        
        
        public void Hide() => ViewObject.SetActive(false);
        public void Show() => ViewObject.SetActive(true);
    }
}
