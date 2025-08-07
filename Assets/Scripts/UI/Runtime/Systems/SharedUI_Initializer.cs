using System;
using Scellecs.Morpeh;
using UI.Requests;
using Unity.IL2CPP.CompilerServices;


namespace UI.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SharedUI_Initializer : IInitializer
    {
        public World World { get; set; }
        

        public void OnAwake()
        {
            var _request = World.GetRequest<SharedUILinkRequest>();

            foreach(var req in _request.Consume()){
                LinkSharedUIToEntities();
            }
        }

        private void LinkSharedUIToEntities()
        {
            // Link ui-widgets refs to entities
        }

        public void Dispose()
        {

        }
    }
}

