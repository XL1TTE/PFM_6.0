using Gameplay.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StateMachineInitializer : IInitializer
    {
        public World World { get; set; }        
        
        public void OnAwake()
        {
            Entity initialState = World.CreateEntity();
            
            var stash_currentState = World.GetStash<CurrentStateComponent>();

            stash_currentState.Set(initialState, new CurrentStateComponent{});
        }

        public void Dispose()
        {

        }
    }
}


