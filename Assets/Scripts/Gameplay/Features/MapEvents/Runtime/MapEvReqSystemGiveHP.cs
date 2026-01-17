using Domain.MapEvents.Requests;
using Domain.Monster.Mono;
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
    public sealed class MapEvReqSystemGiveHP : ISystem
    {
        public World World { get; set; }

        public Request<NormilizeMonsterInfoRequest> req_normilize;
        public Request<GiveHPRequest> req_give_hp;
        public void OnAwake()
        {
            req_give_hp =
                World.GetRequest<GiveHPRequest>();
            req_normilize =
                World.GetRequest<NormilizeMonsterInfoRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_give_hp.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(GiveHPRequest req)
        {
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            var tmp_copy = new List<MonsterData>(crusadeMonsters.crusade_monsters);
            var tmp_copy_copy = new List<MonsterData>();

            List<int> availableIndices = new List<int>();
            for (int i = 0; i < tmp_copy.Count; i++)
            {
                availableIndices.Add(i);
            }

            for (int i = 0; i < Mathf.Min(req.monster_count, tmp_copy.Count); i++) {
                // Pick the first unique index and remove it
                int randomIndex = Random.Range(0, tmp_copy.Count-i);
                int index = availableIndices[randomIndex];
                availableIndices.RemoveAt(randomIndex);

                tmp_copy_copy.Add(tmp_copy[index]);
            }

            foreach (var monster in tmp_copy_copy)
            {
                if (req.use_percent)
                {
                    monster.current_hp = (int)Mathf.Floor(monster.current_hp + monster.max_hp * req.amount_percent);
                }
                else
                {
                    monster.current_hp = (int)(monster.current_hp + req.amount_flat);
                }

                monster.current_hp = Mathf.Clamp(monster.current_hp, 0, monster.max_hp);

                foreach (var main_monst in tmp_copy)
                {
                    if (main_monst.m_MonsterName == monster.m_MonsterName)
                    {
                        main_monst.current_hp = monster.current_hp;
                        break;
                    }
                }
            }


            req_normilize.Publish(new NormilizeMonsterInfoRequest());


            crusadeMonsters.crusade_monsters = tmp_copy;
        }
    }

}