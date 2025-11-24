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


        [Header("Containers")]
        [SerializeField] private Transform m_TurnTakerPageRoot;
        [SerializeField] private Transform m_HoveredEntityPageRoot;

        [Header("TurnTaker Info")]
        [SerializeField] private TextMeshProUGUI m_NameFirstLatter;
        [SerializeField] private TextMeshProUGUI m_NameRemains;
        [SerializeField] public Image m_Avatar;

        [Header("Turn Taker Stats")]
        [SerializeField] private TextMeshProUGUI m_Health;
        [SerializeField] private TextMeshProUGUI m_Speed;
        [SerializeField] private Image m_FireRes;
        [SerializeField] private Image m_PoisonRes;
        [SerializeField] private Image m_BleedingRes;

        [Header("Hovered entity Info")]
        [SerializeField] private TextMeshProUGUI m_RpNameFirstLatter;
        [SerializeField] private TextMeshProUGUI m_RpNameRemains;
        [SerializeField] public Image m_RpAvatar;

        [Header("Hovered Entity Stats")]
        [SerializeField] private TextMeshProUGUI m_RpHealth;
        [SerializeField] private TextMeshProUGUI m_RpSpeed;
        [SerializeField] private Image m_RpFireRes;
        [SerializeField] private Image m_RpPoisonRes;
        [SerializeField] private Image m_RpBleedingRes;

        public void HideTurnTakerInfo()
        {
            m_TurnTakerPageRoot.gameObject.SetActive(false);

        }
        public void ShowTurnTakerInfo()
        {
            m_TurnTakerPageRoot.gameObject.SetActive(true);
        }
        public void HideHoveredEntityInfo()
        {
            m_HoveredEntityPageRoot.gameObject.SetActive(false);

        }
        public void ShowHoveredEntityInfo()
        {
            m_HoveredEntityPageRoot.gameObject.SetActive(true);

        }

        public void SetHealth(int a_maxHealth, int a_health)
        {
            m_Health.text = $"{a_health}/{a_maxHealth}";
        }

        public void SetSpeed(int a_speed) => m_Speed.text = $"{a_speed}";

        public void SetFireResSprite(Sprite sprite) => m_FireRes.sprite = sprite;
        public void SetPoisonResSprite(Sprite sprite) => m_PoisonRes.sprite = sprite;
        public void SetBleedResSprite(Sprite sprite) => m_BleedingRes.sprite = sprite;

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
