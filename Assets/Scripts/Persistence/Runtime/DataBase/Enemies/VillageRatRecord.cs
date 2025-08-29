using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    public sealed class VillageRatRecord: IDbRecord{
        public VillageRatRecord(){
            With<ID>(new ID{Value = "e_VillageRat"});
            With<PrefabComponent>(new PrefabComponent { 
                Value = "Enemies/p_Village_RatEnemy".LoadResource<GameObject>()});
        }
    }
    
}
