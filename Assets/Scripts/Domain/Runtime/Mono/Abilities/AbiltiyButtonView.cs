using UnityEngine;

namespace Domain.Abilities.Mono
{
    public abstract class AbiltiyButtonView: MonoBehaviour{
        [SerializeField] protected GameObject HoverLayer;
        [SerializeField] protected GameObject SelectedLayer;
        [SerializeField] protected GameObject UnavaibleLayer;

        public virtual void EnableHoverView() => HoverLayer?.SetActive(true);
        public virtual void DisableHoverView() => HoverLayer?.SetActive(false);
        public virtual void EnableSelectedView() => SelectedLayer?.SetActive(true);
        public virtual void DisableSelectiedView() => SelectedLayer?.SetActive(false);
        public virtual void EnableUnavaibleView() => UnavaibleLayer?.SetActive(true);
        public virtual void DisableUnavaibleView() => UnavaibleLayer?.SetActive(false);
    }
}
