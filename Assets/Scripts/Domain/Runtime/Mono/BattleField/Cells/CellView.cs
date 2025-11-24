using UnityEngine;

namespace Domain.BattleField
{

    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] GameObject m_SelectedLayer;
        [SerializeField] GameObject m_HoverLayer;
        [SerializeField] GameObject m_PointerLayer;
        [SerializeField] GameObject m_ForbbidenLayer;

        [SerializeField] public Transform m_HealthBarContainer;

        public void EnableSelectedLayer() => m_SelectedLayer?.SetActive(true);
        public void DisableSelectedLayer() => m_SelectedLayer?.SetActive(false);
        public void EnableHoverLayer() => m_HoverLayer?.SetActive(true);
        public void DisableHoverLayer() => m_HoverLayer?.SetActive(false);
        public void EnablePointerLayer() => m_PointerLayer?.SetActive(true);
        public void DisablePointerLayer() => m_PointerLayer?.SetActive(false);
        public void EnableForbbidenLayer() => m_ForbbidenLayer?.SetActive(true);
        public void DisableForbbidenLayer() => m_ForbbidenLayer?.SetActive(false);
    }
}
