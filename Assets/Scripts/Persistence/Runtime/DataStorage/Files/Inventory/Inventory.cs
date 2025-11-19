

using Domain.Monster.Mono;
using System.Collections.Generic;

namespace Persistence.DS
{

    public sealed class InventoryFile : DataStorage.IStorageFile
    {
        public InventoryFile()
        {
            ID<Inventory>(); 
            With<MonstersStorage>(new MonstersStorage() { max_capacity = 5, storage_monsters = new List<MonsterData>() });
            With<BodyPartsStorage>(new BodyPartsStorage() { });
            With<ResourcesStorage>(new ResourcesStorage() { });
        }
    }

    public struct Inventory : IDataStoragaFileID { }
}
