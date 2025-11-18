using System.Linq;
using Domain.Extentions;
using TMPro;
using Unity.VisualScripting;
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

        [Header("TurnTaker")]
        [SerializeField] private TextMeshProUGUI m_NameFirstLatter;
        [SerializeField] private TextMeshProUGUI m_NameRemains;
        [SerializeField] public Image m_Avatar;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI m_Health;
        [SerializeField] private TextMeshProUGUI m_Speed;


        public void SetHealth(int a_maxHealth, int a_health)
        {
            m_Health.text = $"{a_health}/{a_maxHealth}";
        }

        public void SetSpeed(int a_speed) => m_Speed.text = $"{a_speed}";

        public void SetTurnTakerName(string a_name)
        {
            if (a_name == null || a_name == "")
            {
                m_NameFirstLatter.text = "U";
                m_NameRemains.text = "nknown";


                return;
            }

            m_NameFirstLatter.text = $"{a_name.First()}";
            m_NameRemains.text = a_name.Substring(1);
        }

        public void SpawnStartBattleButton()
        {
            BattleStartButtonInstance = Instantiate(BattleStartButtonPrefab, StartBattleButtonSlot);
        }
    }
}
