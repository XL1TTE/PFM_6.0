using Domain.AbilityGraph;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AbilityGraph{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityGraphExecutionCleanup : ICleanupSystem
    {
        private Filter f_abilitiesToCleanup;

        public World World { get; set ; }

        public void OnAwake()
        {
            f_abilitiesToCleanup = World.Filter
                .With<AbilityExecutionGraph>()
                .With<AbilityExecutionState>()
                .With<AbilityExecutionCompletedTag>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var ability in f_abilitiesToCleanup){
                World.RemoveEntity(ability);
            }
        }
        public void Dispose()
        {
        }

    }
}
