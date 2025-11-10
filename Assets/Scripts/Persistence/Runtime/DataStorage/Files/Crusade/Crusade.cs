

namespace Persistence.DS
{

    public sealed class CrusadeFile : DataStorage.IStorageFile
    {
        public CrusadeFile()
        {
            ID<Crusade>();
            With<CrusadeState>(new CrusadeState() { crusade_state = CRUSADE_STATE.NONE });
            With<MapNodes>(new MapNodes());
            With<MapBGs>(new MapBGs());
            With<CurrentBattleId>(new CurrentBattleId() { battle_event_id = "" });
        }
    }

    public struct Crusade : IDataStoragaFileID { }
}
