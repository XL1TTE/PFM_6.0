using System;
using Domain.Events;
using Domain.Extentions;
using Domain.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

namespace Core.Utilities.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EntityPrefabInstantiateSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<EntityPrefabInstantiateRequest> req_entityPfbInstansiate;
        private Event<EntityPrefabInstantiatedEvent> evt_entityPfbInstantiated;

        public void OnAwake()
        {
            req_entityPfbInstansiate =
                World.GetRequest<EntityPrefabInstantiateRequest>();
                 
            evt_entityPfbInstantiated =
                World.GetEvent<EntityPrefabInstantiatedEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_entityPfbInstansiate.Consume()){
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }

        private void ProcessRequest(EntityPrefabInstantiateRequest req)
        {
            GameObject Entity;
            if(req.Parent != null){
                Entity = UnityEngine.Object.Instantiate(req.EntityPrefab, req.Parent);
            }
            else{
                Entity = UnityEngine.Object.Instantiate(req.EntityPrefab);
            }
            if(Entity.TryFindComponent<EntityProvider>(out var unknown) == false){
                throw new Exception("You tried to instantiate Entity prefab, but no Entity providers was found on it.");
            }
            evt_entityPfbInstantiated.NextFrame(new EntityPrefabInstantiatedEvent{
               GUID = req.GUID,
               EntityProvider = unknown
            });
        }
    }
}


