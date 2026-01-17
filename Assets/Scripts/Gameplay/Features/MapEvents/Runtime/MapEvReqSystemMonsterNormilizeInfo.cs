using Domain.MapEvents.Requests;
using Domain.Monster.Mono;
using Domain.Stats.Components;
using Persistence.Components;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


namespace Gameplay.MapEvents.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapEvReqSystemMonsterNormilizeInfo : ISystem
    {
        public World World { get; set; }

        public Request<NormilizeMonsterInfoRequest> req_normilize;
        public void OnAwake()
        {
            req_normilize =
                World.GetRequest<NormilizeMonsterInfoRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_normilize.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(NormilizeMonsterInfoRequest req)
        {
            RemoveDeadMonstersFromAllStorage();
            UpdateMonsterData();
        }

        private void UpdateMonsterData()
        {
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            var tmp_copy_crusade = new List<MonsterData>(crusadeMonsters.crusade_monsters);

            ref var allMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            var tmp_copy_inventory = new List<MonsterData>(allMonsters.storage_monsters);

            foreach (var monster in tmp_copy_crusade)
            {
                // update current bodyparts
                for (int i = 0; i < tmp_copy_inventory.Count; i++)
                {
                    if (tmp_copy_inventory[i].m_MonsterName == monster.m_MonsterName)
                    {
                        tmp_copy_inventory[i] = monster;
                        break;
                    }
                }


                // check for max hp change
                int total_new_max = 0;
                foreach (var part in monster.all_parts_list)
                {
                    if (Persistence.DB.DataBase.TryFindRecordByID(part, out var e_record))
                    {
                        if (Persistence.DB.DataBase.TryGetRecord<EffectsProvider>(e_record, out var e_effects_record))
                        {
                            if (Persistence.DB.DataBase.TryFindRecordByID(e_effects_record.m_Effects[0], out var e_effect_pointer))
                            {
                                if (Persistence.DB.DataBase.TryGetRecord<MaxHealthModifier>(e_effect_pointer, out var e_effect_hp))
                                {
                                    total_new_max += e_effect_hp.m_Flat;
                                }
                            }
                        }
                    }
                }
                monster.max_hp = total_new_max;

                // clamp hp
                monster.current_hp = Mathf.Clamp(monster.current_hp, 0, monster.max_hp);
            }
            crusadeMonsters.crusade_monsters = tmp_copy_crusade;
            allMonsters.storage_monsters = tmp_copy_inventory;
        }
        private void RemoveDeadMonstersFromAllStorage()
        {
            ref var allMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            var tmp_copy_inventory = new List<MonsterData>(allMonsters.storage_monsters);

            var tmp_copy_crusade = new List<MonsterData>();
            foreach(var monster in crusadeMonsters.crusade_monsters)
            {
                if (monster.current_hp > 0)
                {
                    tmp_copy_crusade.Add(monster);
                }
            }
            crusadeMonsters.crusade_monsters = tmp_copy_crusade;

            foreach (var monster in allMonsters.storage_monsters)
            {
                bool tmp_do_not_exist = true;
                foreach (var crusade_monster in crusadeMonsters.crusade_monsters)
                {
                    if (monster.m_MonsterName == crusade_monster.m_MonsterName)
                    {
                        tmp_do_not_exist = false;
                        break;
                    }
                }

                if (tmp_do_not_exist)
                {
                    tmp_copy_inventory.Remove(monster);
                }
            }

            allMonsters.storage_monsters = tmp_copy_inventory;
        }
    }

}