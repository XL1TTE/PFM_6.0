using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{

    public sealed class ev_BattleDefault : IDbRecord
    {
        public ev_BattleDefault()
        {
            ID("ev_BattleDefault");

            With<ID>(new ID { m_Value = "ev_BattleDefault" });
            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village".LoadResource<GameObject>()
            });
            //With<RewardsPool>(new RewardsPool
            //{
            //    Value = new string[1]{
            //        //"e_VillageRat", 2, 1, chance, [1,2], 0.3f
            //        "e_VillageRat"
            //    }
            //});
            //With<EnemiesCount>(new EnemiesCount
            //{
            //    Value = 99
            //});
        }
    }
    public sealed class ev_BattleTest1 : IDbRecord
    {
        public ev_BattleTest1()
        {
            ID("ev_BattleTest1");

            With<ID>(new ID { m_Value = "ev_BattleTest1" });
            With<MapEvBattleTag>(new MapEvBattleTag { });

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

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village".LoadResource<GameObject>()
            });
            //With<RewardsPool>(new RewardsPool
            //{
            //    Value = new string[1]{
            //        //"e_VillageRat", 2, 1, chance, [1,2], 0.3f
            //        "e_VillageRat"
            //    }
            //});
            //With<EnemiesCount>(new EnemiesCount
            //{
            //    Value = 99
            //});
        }
    }
    public sealed class ev_BattleTest2 : IDbRecord
    {
        public ev_BattleTest2()
        {
            ID("ev_BattleTest2");

            With<ID>(new ID { m_Value = "ev_BattleTest2" });

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
