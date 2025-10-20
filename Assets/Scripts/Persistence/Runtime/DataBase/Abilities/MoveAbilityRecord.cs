
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class MoveAbilityRecord : MonsterPartRecord
    {
        public MoveAbilityRecord()
        {

            ID("abt_moveAbility");

            With<ID>(new ID { Value = "abt_moveAbility" });
            With<PrefabComponent>(
                new PrefabComponent { Value = "Abilities/MoveAbilityButton".LoadResource<GameObject>() });
        }
    }
}

