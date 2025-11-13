using Persistence.DS;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Files
{
    public struct BattleConfig : IDataStoragaFileID
    {
       
    }

    public struct LoadConfig : IDataStorageRecord {
        public GameObject m_prefab_level;
    }
}
