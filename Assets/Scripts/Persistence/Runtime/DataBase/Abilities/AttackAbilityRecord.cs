
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class AttackAbilityRecord : MonsterPartRecord
    {
        public AttackAbilityRecord()
        {

            ID("abt_attackAbility");

            With<ID>(new ID { Value = "abt_attackAbility" });
            With<PrefabComponent>(
                new PrefabComponent { Value = "Abilities/AttackAbilityButton".LoadResource<GameObject>() });
        }
    }
}

