using System;
using Domain.UI.Components;
using Domain.UI.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace UI.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SharedUI_Initializer : ISystem
    {
        public World World { get; set; }
        
        private Entity _sharedUIEntity;
        private Stash<SharedUIRefsComponent> stash_sharedUIrefs;

        public void OnAwake()
        {
            var _request = World.GetRequest<SharedUILinkRequest>();
            stash_sharedUIrefs = World.GetStash<SharedUIRefsComponent>();
            foreach (var req in _request.Consume()){
                LinkSharedUIToEntities(req);
            }
        }

        private void LinkSharedUIToEntities(SharedUILinkRequest req)
        {
            _sharedUIEntity = World.CreateEntity();
            ref var refs = ref stash_sharedUIrefs.Add(_sharedUIEntity);
            
            refs.FullScreenNotification = req.ref_FullScreenNotification;
            refs.FpsCounter = req.ref_FpsCounter;
        }

        public void Dispose()
        {

        }

        public void OnUpdate(float deltaTime)
        {

        }
    }
}

