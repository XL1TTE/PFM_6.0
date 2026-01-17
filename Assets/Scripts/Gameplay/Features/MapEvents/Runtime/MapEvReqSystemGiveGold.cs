using Domain.MapEvents.Requests;
using Persistence.DS;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.MapEvents.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapEvReqSystemGiveGold : ISystem
    {
        public World World { get; set; }

        public Request<GiveGoldRequest> req_give_gold;
        public void OnAwake()
        {
            req_give_gold =
                World.GetRequest<GiveGoldRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_give_gold.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(GiveGoldRequest req)
        {
            ref var bodyResourcesStorage = ref DataStorage.GetRecordFromFile<Inventory, ResourcesStorage>();
            bodyResourcesStorage.gold += (int)req.amount;
        }
    }

}