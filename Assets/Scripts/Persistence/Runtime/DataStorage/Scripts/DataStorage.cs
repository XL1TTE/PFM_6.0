using System;
using System.Linq;
using Core.Utilities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;



namespace Persistence.DS
{

    public static class DataStorage
    {
        public class IStorageFile
        {
            internal Entity m_FileEntity;

            protected IStorageFile()
            {
                m_FileEntity = DataStorage.CreateNewFileEntity();
            }
            internal IStorageFile(int a_fileHashId)
            {
                m_FileEntity = DataStorage.CreateNewFileEntity();
                DataStorage.RegisterFileWithID(a_fileHashId, this);
            }

            protected void ID<ID>() where ID : struct, IDataStoragaFileID
            {
                DataStorage.RegisterFileWithID(typeof(ID).GetHashCode(), this);
            }

            protected void With<T>(T a_value) where T : struct, IDataStorageRecord
            {
                DataStorage.AddRecordToFile<T>(this, a_value);
            }

            public ref T GetRecord<T>() where T : struct, IDataStorageRecord
            {
                return ref DataStorage.GetRecordFromFile<T>(this);
            }

            public void SetRecord<T>(T a_value) where T : struct, IDataStorageRecord
            {
                DataStorage.SetRecordInFile<T>(this, a_value);
            }
        }

        public sealed class StorageFileBuilder
        {
            private IStorageFile m_File;

            internal StorageFileBuilder(int a_fileHashId)
            {
                m_File = new IStorageFile(a_fileHashId);
            }

            public StorageFileBuilder WithRecord<T>(T a_value) where T : struct, IDataStorageRecord
            {
                DataStorage.AddRecordToFile<T>(m_File, a_value);
                return this;
            }

            public IStorageFile Build() => m_File;
        }

        private static readonly World m_storageWorld;

        private static bool _isInitialized = false;

        private static IntHashMap<IStorageFile> m_FilesMap = new();

        static DataStorage()
        {
            if (m_storageWorld == null) { m_storageWorld = World.Create(); }
        }

        public static void Initialize()
        {
            if (_isInitialized == false)
            {
                Init();
                _isInitialized = true;
            }
            else { return; }
        }

        private static void Init()
        {
            var t_files = ReflectionUtility.FindAllSubclasses<IStorageFile>();

            foreach (var file in t_files)
            {
                Activator.CreateInstance(file); // Just need to call contructor
            }
            Commit();
        }

        private static void Commit()
        {
            m_storageWorld?.Commit();
        }



        /// <summary>
        /// Storage file builder method.
        /// </summary>
        /// <returns>StorageFile builder.</returns>
        public static StorageFileBuilder NewFile<ID>()
            where ID : struct, IDataStoragaFileID
            => new StorageFileBuilder(typeof(ID).GetHashCode());

        public static IStorageFile GetFile<ID>()
            where ID : struct, IDataStoragaFileID
        {
            var t_file = m_FilesMap.TryGetValueRefByKey(typeof(ID).GetHashCode(), out var t_isExist);
            if (t_isExist)
            {
                return t_file;
            }
            return null;
        }

        public static ref T GetRecordFromFile<ID, T>()
        where ID : struct, IDataStoragaFileID
        where T : struct, IDataStorageRecord
        {
            var t_file = GetFile<ID>();
            if (t_file != null)
            {
                return ref t_file.GetRecord<T>();
            }
            throw new Exception($"You tryied to get record from not existing file.\n\tProvider id: {typeof(ID)};");
        }

        private static void AddRecordToFile<T>(IStorageFile a_file, T a_value) where T : struct, IDataStorageRecord
        {
            var t_stash = m_storageWorld.GetStash<T>();
            t_stash.Set(a_file.m_FileEntity, a_value);

            Commit();
        }
        private static void RemoveRecordFromFile<T>(IStorageFile a_file, T a_value) where T : struct, IDataStorageRecord
        {
            var t_stash = m_storageWorld.GetStash<T>();
            if (t_stash.Has(a_file.m_FileEntity))
            {
                t_stash.Remove(a_file.m_FileEntity);
            }

            Commit();
        }
        private static ref T GetRecordFromFile<T>(IStorageFile a_file)
        where T : struct, IDataStorageRecord
        {
            var t_stash = m_storageWorld.GetStash<T>();
            if (t_stash.Has(a_file.m_FileEntity))
            {
                return ref t_stash.Get(a_file.m_FileEntity);
            }
            throw new Exception("You are trying to get not existing record from file.");
        }
        private static void SetRecordInFile<T>(IStorageFile a_file, T a_value) where T : struct, IDataStorageRecord
        {
            var t_stash = m_storageWorld.GetStash<T>();
            t_stash.Set(a_file.m_FileEntity, a_value);
        }
        /// <summary>
        /// Data storage internal method. Now for external access.
        /// </summary>
        /// <returns>File entity.</returns>
        private static Entity CreateNewFileEntity()
        {
            return m_storageWorld.CreateEntity();
        }
        private static void RegisterFileWithID(int a_hashID, IStorageFile a_file)
        {
            m_FilesMap.Add(a_hashID, a_file, out var slotIndex);
            Commit();
        }

    }
}

