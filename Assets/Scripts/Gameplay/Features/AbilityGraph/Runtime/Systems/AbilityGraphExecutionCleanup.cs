using Domain.AbilityGraph;
using Domain.Commands;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AbilityGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityGraphExecutionCleanup : ICleanupSystem
    {
        private Filter f_abilitiesToCleanup;
        private Event<AbilityExecutionEnded> evt_AbilityExecutionEnded;
        private Stash<AbilityCasterComponent> stash_caster;

        public World World { get; set; }


        public void OnAwake()
        {
            evt_AbilityExecutionEnded = World.GetEvent<AbilityExecutionEnded>();

            stash_caster = World.GetStash<AbilityCasterComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_AbilityExecutionEnded.publishedChanges)
            {
                World.RemoveEntity(evt.m_Ability);
            }
        }

        public void Dispose()
        {
        }

    }
}
