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
    public sealed class MapEvReqSystemSwapPartsBetween : ISystem
    {
        public World World { get; set; }

        public Request<NormilizeMonsterInfoRequest> req_normilize;
        public Request<SwapPartsBetweenMonstersRequest> req_swap_parts_between;
        public void OnAwake()
        {
            req_swap_parts_between =
                World.GetRequest<SwapPartsBetweenMonstersRequest>();
            req_normilize =
                World.GetRequest<NormilizeMonsterInfoRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_swap_parts_between.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(SwapPartsBetweenMonstersRequest req)
        {
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            var tmp_copy = new List<MonsterData>(crusadeMonsters.crusade_monsters);
            var tmp_copy_copy = new List<MonsterData>(crusadeMonsters.crusade_monsters);

            MonsterData mon1;
            MonsterData mon2;

            if (crusadeMonsters.crusade_monsters.Count <= 1)
            {
                return;
            }

            if (crusadeMonsters.crusade_monsters.Count == 2)
            {
                mon1 = crusadeMonsters.crusade_monsters[0];
                mon2 = crusadeMonsters.crusade_monsters[1];
            }
            else
            {
                List<int> availableIndices = new List<int>();
                for (int i = 0; i < tmp_copy.Count; i++)
                {
                    availableIndices.Add(i);
                }

                // Pick the first unique index and remove it
                int randomIndex1 = Random.Range(0, tmp_copy_copy.Count);
                int index1 = availableIndices[randomIndex1];
                availableIndices.RemoveAt(randomIndex1);

                // Pick the second unique index from the remaining
                int randomIndex2 = Random.Range(0, tmp_copy_copy.Count);
                int index2 = availableIndices[randomIndex2];
                // availableIndices.RemoveAt(randomIndex2); // Not needed as we only need 2

                mon1 = tmp_copy[index1];
                mon2 = tmp_copy[index2];
            }


            Debug.Log($"{mon1.m_MonsterName} swaps with {mon2.m_MonsterName}");
            foreach (var part_type in req.parts_types)
            {
                string tmp_part_id = "";
                switch (part_type)
                {
                    case BODYPART_SPECIFIED_TYPE.HEAD:
                        tmp_part_id = mon1.Head_id;
                        mon1.Head_id = mon2.Head_id;
                        mon2.Head_id = tmp_part_id;
                        break;
                    case BODYPART_SPECIFIED_TYPE.TORSO:
                        tmp_part_id = mon1.Body_id;
                        mon1.Body_id = mon2.Body_id;
                        mon2.Body_id = tmp_part_id;
                        break;
                    case BODYPART_SPECIFIED_TYPE.LARM:
                        tmp_part_id = mon1.NearArm_id;
                        mon1.NearArm_id = mon2.NearArm_id;
                        mon2.NearArm_id = tmp_part_id;
                        break;
                    case BODYPART_SPECIFIED_TYPE.RARM:
                        tmp_part_id = mon1.FarArm_id;
                        mon1.FarArm_id = mon2.FarArm_id;
                        mon2.FarArm_id = tmp_part_id;
                        break;
                    case BODYPART_SPECIFIED_TYPE.LLEG:
                        tmp_part_id = mon1.NearLeg_id;
                        mon1.NearLeg_id = mon2.NearLeg_id;
                        mon2.NearLeg_id = tmp_part_id;
                        break;
                    case BODYPART_SPECIFIED_TYPE.RLEG:
                        tmp_part_id = mon1.FarLeg_id;
                        mon1.FarLeg_id = mon2.FarLeg_id;
                        mon2.FarLeg_id = tmp_part_id;
                        break;
                }
            }


            req_normilize.Publish(new NormilizeMonsterInfoRequest());


            crusadeMonsters.crusade_monsters = tmp_copy;
        }
    }

}