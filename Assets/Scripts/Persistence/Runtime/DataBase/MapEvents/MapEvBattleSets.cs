using Domain.Extentions;
using Domain.Levels.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class ev_BattleLevel_Village_1 : IDbRecord
    {
        public ev_BattleLevel_Village_1()
        {
            ID("ev_BattleLevel_Village_1");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_1".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 4
            });
        }
    }
    public sealed class ev_BattleLevel_Village_1_1 : IDbRecord
    {
        public ev_BattleLevel_Village_1_1()
        {
            ID("ev_BattleLevel_Village_1_1");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_1_1".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 4
            });
        }
    }
    public sealed class ev_BattleLevel_Village_2 : IDbRecord
    {
        public ev_BattleLevel_Village_2()
        {
            ID("ev_BattleLevel_Village_2");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_2".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 4
            });
        }
    }
    public sealed class ev_BattleLevel_Village_2_1 : IDbRecord
    {
        public ev_BattleLevel_Village_2_1()
        {
            ID("ev_BattleLevel_Village_2_1");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_2_1".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 4
            });
        }
    }
    public sealed class ev_BattleLevel_Village_3 : IDbRecord
    {
        public ev_BattleLevel_Village_3()
        {
            ID("ev_BattleLevel_Village_3");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_3".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 3
            });
        }
    }
    public sealed class ev_BattleLevel_Village_4 : IDbRecord
    {
        public ev_BattleLevel_Village_4()
        {
            ID("ev_BattleLevel_Village_4");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_4".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 2
            });
        }
    }
    public sealed class ev_BattleLevel_Village_5 : IDbRecord
    {
        public ev_BattleLevel_Village_5()
        {
            ID("ev_BattleLevel_Village_5");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_5".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 1
            });
        }
    }
    public sealed class ev_BattleLevel_Village_6 : IDbRecord
    {
        public ev_BattleLevel_Village_6()
        {
            ID("ev_BattleLevel_Village_6");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_6".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 3
            });
        }
    }
    public sealed class ev_BattleLevel_Village_7 : IDbRecord
    {
        public ev_BattleLevel_Village_7()
        {
            ID("ev_BattleLevel_Village_7");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_7".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 2
            });
        }
    }
    public sealed class ev_BattleLevel_Village_8 : IDbRecord
    {
        public ev_BattleLevel_Village_8()
        {
            ID("ev_BattleLevel_Village_8");

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/Level_Village_8".LoadResource<GameObject>()
            });

            With<EnemiesCount>(new EnemiesCount
            {
                amount = 3
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
    public sealed class ev_BattleDefault : IDbRecord
    {
        public ev_BattleDefault()
        {
            ID("ev_BattleDefault");

            //With<ID>(new ID { m_Value = "ev_BattleDefault" });
            With<MapEvBattleTag>(new MapEvBattleTag { });
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            // ������ ������
            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village 2".LoadResource<GameObject>()
            });

            // ���� ��� ��� �����. ����� ������ ����� ���������� �����
            //With<RewardsPool>(new RewardsPool
            //{
            //    Value = new string[1]{
            //              "SOME BODYPART", "N AMOUNT OF GOLD"
            //    }
            //});

            // ��������� ���-�� �����������
            With<EnemiesCount>(new EnemiesCount
            {
                amount = 2
            });
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
    public sealed class ev_BattleTutorial : IDbRecord
    {
        public ev_BattleTutorial()
        {
            ID("ev_BattleTutorial");

            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });

            With<MapEvBattleTag>(new MapEvBattleTag { });

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Levels/lvl_Village".LoadResource<GameObject>()
            });
        }
    }
}
