using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Domain.UI.Mono
{
    public class NextTurnBtnView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject ViewObject;
        [SerializeField] private Collider2D m_Collider;
        [SerializeField] private float AnimationDuration;
        [SerializeField] private Ease AnimationEase;

        private Vector2 OriginalPosition;

        void Awake()
        {
            if (ViewObject != null)
            {
                OriginalPosition = ViewObject.transform.position;
            }

            PlayAppearAnimation();
        }

        private Tween _animation;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_animation != null)
            {
                _animation.Kill(false);
            }
            if (ViewObject == null) { return; }
            _animation = ViewObject.transform
                .DOMoveY(OriginalPosition.y + 7.0f, AnimationDuration)
                .SetEase(AnimationEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_animation != null)
            {
                _animation.Kill(false);
            }
            if (ViewObject == null) { return; }
            _animation = ViewObject.transform
                .DOMoveY(OriginalPosition.y, AnimationDuration)
                .SetEase(AnimationEase);
        }

        private void DisableView()
        {
            ViewObject.SetActive(false);
        }

        private void DisableAnimations()
        {
            this.gameObject.SetActive(false);
        }
        private void EnableView()
        {
            ViewObject.SetActive(true);
        }

        private void EnableAnimations()
        {
            this.gameObject.SetActive(true);
        }

        private void PlayAppearAnimation()
        {
            if (_animation != null)
            {
                _animation.Kill(false);
            }

            ViewObject.transform.position -= new Vector3(0, 20, 0);
            _animation = ViewObject.transform
                .DOMoveY(OriginalPosition.y, 0.5f).OnComplete(
                    () => { EnableAnimations(); }
    );
        }

        public void Hide()
        {
            if (_animation != null)
            {
                _animation.Kill(false);
            }
            DisableAnimations();
            _animation = ViewObject.transform
                .DOMoveY(OriginalPosition.y - 40, 0.25f).OnComplete(
                    () => { DisableView(); }
                );
        }
        public void Show()
        {
            EnableView();
            PlayAppearAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                G.NextTurn(ECS.m_CurrentWorld);
            }
        }
    }
}
