using Domain.MapEvents.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


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
            //ref var resourcesStorage = ref DataStorage.GetRecordFromFile<Inventory, ResourcesStorage>(); //DataStorage - вся изменяемая инфа об игроке
            //в инвентаре ищем штуку 2
            //resourcesStorage.gold += req.amount;

            Debug.Log($"Give gold request is processed, gave {req.amount}");
        }
    }

}