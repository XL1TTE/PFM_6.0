using System.Collections.Generic;
using Domain.Components;
using Domain.Extentions;
using Domain.Levels.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    
    public sealed class VillageLevelRecord: IDbRecord{
        public VillageLevelRecord(){
            With<ID>(new ID{Value = "lvl_Village"});
            With<PrefabComponent>(new PrefabComponent { 
                Value = "Levels/lvl_Village".LoadResource<GameObject>()});
            With<EnemiesPool>(new EnemiesPool{
                Value = new string[1]{
                    "e_VillageRat"
                }
            });
        }
    }
    
}
