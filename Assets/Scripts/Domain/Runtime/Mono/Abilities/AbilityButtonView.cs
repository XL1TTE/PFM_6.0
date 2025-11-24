using UnityEngine;
using UnityEngine.UI;

namespace Domain.Abilities.Mono
{
    public class AbilityButtonView : MonoBehaviour
    {
        [SerializeField] protected GameObject HoverLayer;
        [SerializeField] protected GameObject SelectedLayer;
        [SerializeField] protected GameObject UnavaibleLayer;

        [SerializeField] protected Image m_Icon;
        public virtual void SetIcon(Sprite a_icon)
        {
            m_Icon.sprite = a_icon;
        }

        public virtual void EnableHoverView() => HoverLayer?.SetActive(true);
        public virtual void DisableHoverView() => HoverLayer?.SetActive(false);
        public virtual void EnableSelectedView() => SelectedLayer?.SetActive(true);
        public virtual void DisableSelectiedView() => SelectedLayer?.SetActive(false);
        public virtual void EnableUnavaibleView() => UnavaibleLayer?.SetActive(true);
        public virtual void DisableUnavaibleView() => UnavaibleLayer?.SetActive(false);
    }

}
