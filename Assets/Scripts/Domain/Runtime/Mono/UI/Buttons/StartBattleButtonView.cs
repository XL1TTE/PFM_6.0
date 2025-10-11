using System.Numerics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Domain.UI.Mono
{
    public class StartBattleButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        
        [SerializeField] GameObject RootButtonObject;
        [SerializeField] GameObject ViewObject;
        
        [Header("Animation")]
        [SerializeField, Range(1, 2)] private float HoverPunchPower = 1.1f;
        [SerializeField, Range(0, 2)] private float HoverPunchDuration = 0.25f;
        
        private UnityEngine.Vector3 OriginScale;
        
        private Tween Animation;

        void Awake()
        {
            OriginScale = ViewObject.transform.localScale;
        }

        public void DestroySelf(){
            Animation?.Kill(false);            
            Destroy(RootButtonObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Animation?.Kill();
            Animation = ViewObject.transform.DOScale(
                ViewObject.transform.localScale * HoverPunchPower, HoverPunchDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Animation?.Kill();
            Animation = ViewObject.transform.DOScale(
                OriginScale, HoverPunchDuration);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Animation?.Kill();
            Animation = ViewObject.transform.DOPunchScale(
                ViewObject.transform.localScale * HoverPunchPower * 1.05f, HoverPunchDuration);
        }
    }
}
