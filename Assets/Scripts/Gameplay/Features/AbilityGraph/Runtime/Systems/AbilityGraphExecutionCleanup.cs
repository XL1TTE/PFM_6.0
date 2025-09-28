using Domain.AbilityGraph;
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
        private Event<AbiltiyExecutionCompletedEvent> evt_abtExecutionCompleted;
        private Stash<AbilityCasterComponent> stash_caster;

        public World World { get; set; }


        public void OnAwake()
        {
            f_abilitiesToCleanup = World.Filter
                .With<AbilityExecutionGraph>()
                .With<AbilityExecutionState>()
                .With<AbilityExecutionCompletedTag>()
                .Build();

            evt_abtExecutionCompleted = World.GetEvent<AbiltiyExecutionCompletedEvent>();

            stash_caster = World.GetStash<AbilityCasterComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var ability in f_abilitiesToCleanup)
            {
                NotifyAbilityExecutionCompleted(ability);
                World.RemoveEntity(ability);
            }
        }

        private void NotifyAbilityExecutionCompleted(Entity ability)
        {
            evt_abtExecutionCompleted.NextFrame(new AbiltiyExecutionCompletedEvent
            {
                Caster = stash_caster.Get(ability).caster
            });
        }

        public void Dispose()
        {
        }

    }
}
