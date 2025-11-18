using Domain.UI.Widgets;
using UnityEngine;

namespace Domain.UI.Mono
{
    public class BattleUiRefs : MonoBehaviour
    {
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        private static BattleUiRefs _instance;
        public static BattleUiRefs Instance { get => _instance; }

        [SerializeField] public BattleBookWidget BookWidget;
        [SerializeField] public TurnQueueWidget TurnQueueWidget;
        [SerializeField] public InformationBoardWidget InformationBoardWidget;
        [SerializeField] public NextTurnBtnView m_NextTurnButton;
    }
}
