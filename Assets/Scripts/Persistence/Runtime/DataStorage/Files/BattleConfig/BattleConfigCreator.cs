using Domain.Extentions;
using Persistence.DS;
using UnityEngine;

namespace DS.Files
{
    public sealed class BattleConfigCreator : DataStorage.IStorageFile
    {
        public BattleConfigCreator()
        {
            ID<BattleConfig>();
            With<LoadConfig>(new LoadConfig
            {
                // Level by default
                m_prefab_level = "Levels/lvl_Village".LoadResource<GameObject>()
            });
        }
    }
}
