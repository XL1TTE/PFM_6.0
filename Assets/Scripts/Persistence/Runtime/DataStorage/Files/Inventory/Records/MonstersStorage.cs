
using Domain.Monster.Mono;
using System.Collections.Generic;

namespace Persistence.DS
{
    public struct MonstersStorage : IDataStorageRecord
    {
        public int max_capacity;
        public List<MonsterData> storage_monsters;
    }
}
