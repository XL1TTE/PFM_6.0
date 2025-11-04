

namespace Persistence.DS
{

    public sealed class SettingsFile : DataStorage.IStorageFile
    {
        public SettingsFile()
        {
            ID<Settings>();
            With<VolumeSettings>(new VolumeSettings());
        }
    }

    public struct Settings : IDataStoragaFileID { }
}
