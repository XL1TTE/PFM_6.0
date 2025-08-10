using System;
using Scellecs.Morpeh;
using UI.Components;
using UI.Requests;
using Unity.IL2CPP.CompilerServices;


namespace UI.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SharedUI_Initializer : IInitializer
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
        }

        public void Dispose()
        {

        }
    }
}

