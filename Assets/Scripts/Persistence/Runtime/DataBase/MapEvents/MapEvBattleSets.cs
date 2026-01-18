using Domain.Extentions;
using Domain.Levels.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class ev_BattleDefault : IDbRecord
    {
        public ev_BattleDefault()
        {
            ID("ev_BattleDefault");

            With<MapEvBattleTag>(new MapEvBattleTag { });
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village 2".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 2
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Ambush : IDbRecord
    {
        public ev_BattleLevel_Village_Ambush()
        {
            ID("ev_BattleLevel_Village_Ambush");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Ambush".LoadResource<GameObject>()
            });
        }
    }

    public sealed class ev_BattleLevel_Village_Bear : IDbRecord
    {
        public ev_BattleLevel_Village_Bear()
        {
            ID("ev_BattleLevel_Village_Bear");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_BearFight".LoadResource<GameObject>()
            });
        }
    }

    public sealed class ev_BattleLevel_Village_Birds : IDbRecord
    {
        public ev_BattleLevel_Village_Birds()
        {
            ID("ev_BattleLevel_Village_Birds");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Birds".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_BurnGen : IDbRecord
    {
        public ev_BattleLevel_Village_BurnGen()
        {
            ID("ev_BattleLevel_Village_BurnGen");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_BurnGen".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Classic : IDbRecord
    {
        public ev_BattleLevel_Village_Classic()
        {
            ID("ev_BattleLevel_Village_Classic");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Classic".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_DogCatRat : IDbRecord
    {
        public ev_BattleLevel_Village_DogCatRat()
        {
            ID("ev_BattleLevel_Village_DogCatRat");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_DogCatRat".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Holes : IDbRecord
    {
        public ev_BattleLevel_Village_Holes()
        {
            ID("ev_BattleLevel_Village_Holes");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Holes".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Horse : IDbRecord
    {
        public ev_BattleLevel_Village_Horse()
        {
            ID("ev_BattleLevel_Village_Horse");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_HorseFight".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Insects : IDbRecord
    {
        public ev_BattleLevel_Village_Insects()
        {
            ID("ev_BattleLevel_Village_Insects");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Insects".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Jumpers : IDbRecord
    {
        public ev_BattleLevel_Village_Jumpers()
        {
            ID("ev_BattleLevel_Village_Jumpers");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Jumpers".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_LadyBugs : IDbRecord
    {
        public ev_BattleLevel_Village_LadyBugs()
        {
            ID("ev_BattleLevel_Village_LadyBugs");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_LadyBugFest".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Micro : IDbRecord
    {
        public ev_BattleLevel_Village_Micro()
        {
            ID("ev_BattleLevel_Village_Micro");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Micro".LoadResource<GameObject>()
            });
        }
    }
    public sealed class ev_BattleLevel_Village_Racoons : IDbRecord
    {
        public ev_BattleLevel_Village_Racoons()
        {
            ID("ev_BattleLevel_Village_Racoons");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_RacoonBros".LoadResource<GameObject>()
            });
        }
    }

    public sealed class ev_BattleTutorial : IDbRecord
    {
        public ev_BattleTutorial()
        {
            ID("ev_BattleTutorial");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village_Tutorial".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 99
            });
        }
    }



    public sealed class ev_BattleTest1 : IDbRecord
    {
        public ev_BattleTest1()
        {
            ID("ev_BattleTest1");

            //With<ID>(new ID { m_Value = "ev_BattleTest1" });
            With<MapEvBattleTag>(new MapEvBattleTag { });
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

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

            //With<ID>(new ID { m_Value = "ev_BattleTest2" });

            With<MapEvBattleTag>(new MapEvBattleTag { });
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
            {
                count_start_from_zero = true,
                count_offset = 2,
                count_offset_percentile = 0.4f
            });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village".LoadResource<GameObject>()
            });
        }
    }

}
