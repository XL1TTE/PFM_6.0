using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Game;
using Scellecs.Morpeh;
using UI.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Domain.Abilities.Mono
{
    public class AbilityViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Image m_Icon;
        [HideInInspector] public AbilityData m_AbilityData;

        [SerializeField] protected GameObject HoverLayer;

        public void OnPointerEnter(PointerEventData eventData)
        {
            EnableHoverView();
            if (m_AbilityData == null) { return; }
            ToolTipManager.ShowTooltip(T.GetAbilityShortTooltip(m_AbilityData));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DisableHoverView();
            ToolTipManager.HideTooltip();
        }

        public virtual void EnableHoverView() => HoverLayer?.SetActive(true);
        public virtual void DisableHoverView() => HoverLayer?.SetActive(false);
    }

}
