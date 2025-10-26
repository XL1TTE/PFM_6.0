using System;
using Core.Utilities;
using Domain.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;


namespace Persistence.DB
{
    public static class DataBase
    {
        private static readonly World _dbWorld;

        private static bool _isInitialized = false;


        private static IntHashMap<Entity> m_RecordsMap = new();


        static DataBase()
        {
            if (_dbWorld == null) { _dbWorld = World.Create(); }
            //stash_emptyRecords = _dbWorld.GetStash<EmptyRecord>();
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
            var records = ReflectionUtility.FindAllSubclasses<IDbRecord>();

            foreach (var rec in records)
            {
                Activator.CreateInstance(rec); // Just need to call contructor
            }


            Commit();
        }

        public static void Commit()
        {
            _dbWorld.Commit();
        }

        public static FilterBuilder Filter
        {
            get
            {
                return _dbWorld.Filter;
            }
        }

        public static bool TryFindRecordByID(string id, out Entity record)
        {
            if (id == String.Empty || id == null)
            {
                record = default;
                return false;
            }

            var id_hash = id.GetHashCode();
            if (m_RecordsMap.Has(id_hash) == false)
            {
                record = default;
                return false;
            }
            record = m_RecordsMap.GetValueRefByKey(id_hash);
            return true;
        }

        public static Entity CreateRecord()
        {
            return _dbWorld.CreateEntity();
        }

        internal static void RegisterRecordWithId(string id, Entity record)
        {
            m_RecordsMap.Add(id.GetHashCode(), record, out var slotIndex);
        }

        public static void SetRecord<T>(Entity entity, T component) where T : struct, IComponent
        {
            var stash = _dbWorld.GetStash<T>();

            if (stash == null) { return; }

            _dbWorld.GetStash<T>().Set(entity, component);
        }

        public static T GetRecord<T>(Entity entity) where T : struct, IComponent
        {
            var stash = _dbWorld.GetStash<T>();

            if (stash == null) { throw new Exception("Record not found."); }

            return stash.Get(entity);
        }
        public static bool TryGetRecord<T>(Entity entity, out T record) where T : struct, IComponent
        {
            var stash = _dbWorld.GetStash<T>();

            if (stash == null) { record = default; return false; }

            if (!stash.Has(entity))
            {
                record = default; return false;
            }

            record = stash.Get(entity);
            return true;
        }
    }
}

