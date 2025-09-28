using Domain.Components;
using Domain.Extentions;
using Domain.Levels.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{

    public sealed class ev_BattleTest1 : IDbRecord{
        public ev_BattleTest1()
        {
            // -------------------------------------------------- ID and TAG
            With<ID>(new ID{Value = "ev_BattleTest1" }); 
            With<MapEvBattleTag>(new MapEvBattleTag { });


            // -------------------------------------------------- Required components
            With<MapEvStageRequirComponent>(new MapEvStageRequirComponent 
            { 
                acceptable_stages = new System.Collections.Generic.List<STAGES> {
                    STAGES.VILLAGE
                }
            });
            With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
            {
                count_start_from_zero = false,
                count_offset = 1,
                count_offset_percentile = 0.2f,
            });


            // -------------------------------------------------- Main information
            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village".LoadResource<GameObject>()
            });
            With<EnemiesPool>(new EnemiesPool
            {
                Value = new string[1]{
                    "e_VillageRat"
                }
            });
        }
    }
    public sealed class ev_BattleTest2 : IDbRecord
    {
        public ev_BattleTest2()
        {
            With<ID>(new ID { Value = "ev_BattleTest2" });
            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
            {
                count_start_from_zero = true,
                count_offset = 2,
                count_offset_percentile = 0.4f
            });
        }
    }
}
