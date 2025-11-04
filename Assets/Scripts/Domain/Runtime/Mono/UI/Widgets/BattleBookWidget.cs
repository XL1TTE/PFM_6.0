using Domain.Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class BattleBookWidget : MonoBehaviour
    {

        [SerializeField] public Transform EndTurnButtonSlot;

        [Header("Start Battle Button")]
        [SerializeField] public Transform StartBattleButtonSlot;
        [SerializeField] public GameObject BattleStartButtonPrefab;
        [HideInInspector] private GameObject BattleStartButtonInstance;

        [Header("Abilities")]
        [SerializeField] public Transform m_FirstHandAbilitySlot;
        [SerializeField] public Transform m_SecondHandAbilitySlot;
        [SerializeField] public Transform m_HeadAbilitySlot;
        [SerializeField] public Transform m_MoveButtonSlot;
        [SerializeField] public Transform m_TurnAroundButtonSlot;

        [Header("Monster")]
        [SerializeField] public TextMeshProUGUI MonsterNameTMP;
        [SerializeField] public Image TurnTakerAvatar;

        public void SpawnStartBattleButton()
        {
            BattleStartButtonInstance = Instantiate(BattleStartButtonPrefab, StartBattleButtonSlot);
        }
    }
}
