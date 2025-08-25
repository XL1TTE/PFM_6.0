using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class TurnQueueElementView: MonoBehaviour
    {
        [SerializeField] private Image Avatar;
        [SerializeField] private GameObject HighlightLayer;
        
        public void SetAvatar(Sprite sprite){
            Avatar.sprite = sprite;
        }
        public void ClearAvatar(){
            Avatar.sprite = null;
        }
        
        public void EnableHighlighting(){
            HighlightLayer.SetActive(true);
        }
        public void DisableHighlighting(){
            HighlightLayer.SetActive(false);
        }
    }
}
