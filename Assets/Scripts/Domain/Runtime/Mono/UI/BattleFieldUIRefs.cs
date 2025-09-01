using Domain.UI.Widgets;
using UnityEngine;

namespace Domain.UI.Mono
{
    public class BattleFieldUIRefs : MonoBehaviour
    {
        void Awake()
        {
            if(_instance == null){
                _instance = this;
            }
        }
        private static BattleFieldUIRefs _instance;
        public static BattleFieldUIRefs Instance{get => _instance;}

        [SerializeField] public BattleBookWidget BookWidget;
        [SerializeField] public TurnQueueWidget TurnQueueWidget;
    }
}
