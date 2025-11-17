using Domain.Ecs;
using Domain.Stats.Components;
using Gameplay.GameEffects;
using Gameplay.GameStats.Systems;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class GameStatsModule : IWorldModule
    {
        public int Priority => -45;

        public void Initialize(World world)
        {
            var sg_GameStats = world.CreateSystemsGroup();

            sg_GameStats.AddSystem(new HealthStatSystem());

            sg_GameStats.AddSystem(new StatModifiersSystem<MaxHealthModifier, MaxHealth>());
            sg_GameStats.AddSystem(new StatModifiersSystem<SpeedModifier, Speed>());

            world.AddSystemsGroup(Priority, sg_GameStats);
        }
    }
}


