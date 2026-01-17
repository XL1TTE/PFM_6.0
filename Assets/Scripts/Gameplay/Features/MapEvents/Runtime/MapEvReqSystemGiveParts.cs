using Domain.MapEvents.Requests;
using Persistence.DS;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.MapEvents.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapEvReqSystemGiveParts : ISystem
    {
        
        public World World { get; set; }

        public Request<GivePartsRequest> req_give_parts;
        public void OnAwake()
        {
            req_give_parts =
                World.GetRequest<GivePartsRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_give_parts.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(GivePartsRequest req)
        {
            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();
            foreach (var part in req.parts_id_and_amount)
            {
                // add bodypart if it is already in the storage, or create a new entry with it;
                if (bodyPartsStorage.parts.ContainsKey(part.Key))
                {
                    bodyPartsStorage.parts[part.Key] += part.Value;
                }
                else
                {
                    bodyPartsStorage.parts.Add(part.Key, part.Value);
                }

            }
        }
    }

}