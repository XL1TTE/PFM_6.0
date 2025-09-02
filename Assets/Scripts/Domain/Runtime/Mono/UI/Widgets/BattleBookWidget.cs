using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class BattleBookWidget : MonoBehaviour
    {
        [SerializeField] public Image TurnTakerAvatar;
        [SerializeField] public Transform EndTurnButtonSlot;
        [SerializeField] public Transform HealButtonSlot;
        [SerializeField] public Transform AttackButtonSlot;
        [SerializeField] public Transform EffectButtonSlot;
        [SerializeField] public Transform MoveButtonSlot;
        [SerializeField] public Transform TurnAroundButtonSlot;
        [SerializeField] public TextMeshProUGUI MonsterNameTMP;
    }
}
