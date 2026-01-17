using Domain.MapEvents.Requests;
using Persistence.DS;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
//using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


namespace Gameplay.MapEvents.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapEvReqSystemTakeGold : ISystem
    {
        public World World { get; set; }

        public Request<TakeGoldRequest> req_take_gold;
        public void OnAwake()
        {
            req_take_gold =
                World.GetRequest<TakeGoldRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_take_gold.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(TakeGoldRequest req)
        {
            ref var bodyResourcesStorage = ref DataStorage.GetRecordFromFile<Inventory, ResourcesStorage>();
            bodyResourcesStorage.gold -= (int)req.amount;
            bodyResourcesStorage.gold = Mathf.Max(0, bodyResourcesStorage.gold);
        }
    }

}