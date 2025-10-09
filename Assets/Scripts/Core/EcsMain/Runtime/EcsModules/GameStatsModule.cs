using Domain.ECS;
using Gameplay.GameStats.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules
{
    public sealed class GameStatsModule : IWorldModule
    {
        public int Priority => -45;

        public void Initialize(World world)
        {
            var sg_GameStats = world.CreateSystemsGroup();
            sg_GameStats.AddSystem(new CurrentStatsFromBaseSystem());

            world.AddSystemsGroup(Priority, sg_GameStats);
        }
    }
}


