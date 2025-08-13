using Gameplay.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleStateInitializer : IInitializer
    {
        public World World {get; set;}
        
        public void OnAwake()
        {
            var stash_battleState = World.GetStash<BattleState>();
            
            var stateEntity = World.CreateEntity();
            stash_battleState.Add(stateEntity);
        }

        public void Dispose()
        {
           
        }
    }
}

