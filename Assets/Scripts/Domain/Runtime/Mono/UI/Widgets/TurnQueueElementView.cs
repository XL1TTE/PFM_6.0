using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class TurnQueueElementView : MonoBehaviour
    {
        [SerializeField] private Image Avatar;
        [SerializeField] private Image FrameImage;
        [SerializeField] private GameObject HighlightLayer;
        [SerializeField] private GameObject HoverVisualLayer;

        public void SetAvatar(Sprite sprite)
        {
            Avatar.sprite = sprite;
        }
        public void ClearAvatar()
        {
            Avatar.sprite = null;
        }

        public void EnableHover()
        {
            HoverVisualLayer.SetActive(true);
        }
        public void DisableHover()
        {
            HoverVisualLayer.SetActive(false);
        }

        public void EnableHighlighting()
        {
            HighlightLayer.SetActive(true);
        }
        public void DisableHighlighting()
        {
            HighlightLayer.SetActive(false);
        }


        public void DestroySelf() => Destroy(gameObject);
    }
}
