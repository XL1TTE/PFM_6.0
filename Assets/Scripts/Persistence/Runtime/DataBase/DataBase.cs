using System;
using Core.Components;
using Core.Utilities;
using Scellecs.Morpeh;

 
namespace Persistence.DB{
    public static class DataBase
    {
        private static readonly World _dbWorld;
        
        private static bool _isInitialized = false;
        
        
        //private static Stash<EmptyRecord> stash_emptyRecords;
        
        static DataBase(){
            if(_dbWorld == null){_dbWorld = World.Create();}
            //stash_emptyRecords = _dbWorld.GetStash<EmptyRecord>();
        }
        
        public static void Initialize(){
            if(_isInitialized == false){
                Init();
                _isInitialized = true;
            }
            else{return;}
        }
        
        private static void Init(){
            var records = ReflectionUtility.FindAllSubclasses<IDbRecord>("Assembly-Persistance");
            
            foreach(var rec in records){
                Activator.CreateInstance(rec); // Just need to call contructor
            }

            _allRecords = _dbWorld.Filter.With<ID>().Build();
            stash_ids = _dbWorld.GetStash<ID>();

            Commit();
        }
        
        public static void Commit(){
            _dbWorld.Commit();
        }

        private static Filter _allRecords;
        private static Stash<ID> stash_ids;

        public static FilterBuilder Filter
        {
            get
            {
                return _dbWorld.Filter;
            }
        }

        public static bool TryFindRecordByID(string id, out Entity record){
            foreach(var r in _allRecords){
                if(stash_ids.Get(r).id == id){record = r; return true;}
            }
            record = default;
            return false;
        }

        public static Entity CreateRecord(){
            return _dbWorld.CreateEntity();
        }
        
        public static void SetRecord<T>(Entity entity, T component) where T: struct, IComponent
        {
            var stash = _dbWorld.GetStash<T>();
            
            if(stash == null){return;}
            
            _dbWorld.GetStash<T>().Set(entity, component);
        }
        
        public static  T GetRecord<T>(Entity entity) where T : struct, IComponent{
            var stash = _dbWorld.GetStash<T>();
            
            if(stash == null){throw new Exception("Record not found.");}
            
            return  stash.Get(entity);  
        }
        public static bool TryGetRecord<T>(Entity entity, out T record) where T : struct, IComponent{
            var stash = _dbWorld.GetStash<T>();
            
            if(stash == null){throw new Exception("Record not found.");}
            
            if(!stash.Has(entity)){
                throw new Exception("Record not found.");
            }
            
            record = stash.Get(entity);
            return true;  
        }
    }
}

