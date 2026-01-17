using Domain.MapEvents.Requests;
using Domain.Monster.Mono;
using Persistence.DS;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


namespace Gameplay.MapEvents.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapEvReqSystemSwapParts : ISystem
    {
        public World World { get; set; }

        public Request<NormilizeMonsterInfoRequest> req_normilize;
        public Request<SwapPartsRequest> req_swap_parts;
        public void OnAwake()
        {
            req_swap_parts =
                World.GetRequest<SwapPartsRequest>();
            req_normilize =
                World.GetRequest<NormilizeMonsterInfoRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_swap_parts.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(SwapPartsRequest req)
        {
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            MonsterData chosen_monster = crusadeMonsters.crusade_monsters[Random.Range(0, crusadeMonsters.crusade_monsters.Count)];
            foreach (var part in req.parts_type_with_id)
            {
                Debug.Log($"{chosen_monster.m_MonsterName} should get {req.parts_type_with_id}");
                switch (part.Key)
                {
                    case BODYPART_SPECIFIED_TYPE.HEAD:
                        chosen_monster.Head_id = part.Value;
                        break;
                    case BODYPART_SPECIFIED_TYPE.TORSO:
                        chosen_monster.Body_id = part.Value;
                        break;
                    case BODYPART_SPECIFIED_TYPE.LARM:
                        chosen_monster.NearArm_id = part.Value;
                        break;
                    case BODYPART_SPECIFIED_TYPE.RARM:
                        chosen_monster.FarArm_id = part.Value;
                        break;
                    case BODYPART_SPECIFIED_TYPE.LLEG:
                        chosen_monster.NearLeg_id = part.Value;
                        break;
                    case BODYPART_SPECIFIED_TYPE.RLEG:
                        chosen_monster.FarLeg_id = part.Value;
                        break;
                }
            }
            req_normilize.Publish(new NormilizeMonsterInfoRequest());
        }
    }

}