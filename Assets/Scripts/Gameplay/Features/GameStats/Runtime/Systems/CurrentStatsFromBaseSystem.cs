using Domain.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.GameStats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CurrentStatsFromBaseSystem : ISystem
    {

        public World World { get; set; }

        private Filter _filter;
        private Stash<BaseStatsComponent> stash_baseStats;
        private Stash<CurrentStatsComponent> stash_curStats;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<BaseStatsComponent>()
                .Without<CurrentStatsComponent>()
                .Build();

            stash_baseStats = World.GetStash<BaseStatsComponent>();
            stash_curStats = World.GetStash<CurrentStatsComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in _filter)
            {
                SetupCurrentStats(e);
            }
        }

        private void SetupCurrentStats(Entity owner)
        {
            var baseStats = stash_baseStats.Get(owner);

            stash_curStats.Set(owner, new CurrentStatsComponent
            {
                CurrentHealth = baseStats.MaxHealth,
                MaxHealth = baseStats.MaxHealth,
                CurrentSpeed = baseStats.MaxSpeed,
                MaxSpeed = baseStats.MaxSpeed
            });
        }

        public void Dispose()
        {

        }
    }
}
