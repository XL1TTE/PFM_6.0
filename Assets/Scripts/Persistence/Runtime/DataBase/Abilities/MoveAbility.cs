
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    public class MoveAbility: MonsterPartRecord{
        public MoveAbility(){
            With<ID>(new ID { id = "abt_moveAbility" });
            With<PrefabComponent>(
                new PrefabComponent{Value = "Abilities/MoveAbility".LoadResource<GameObject>()});
        }
    }
}

