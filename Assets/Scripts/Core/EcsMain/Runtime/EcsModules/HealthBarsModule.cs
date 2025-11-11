using Domain.Ecs;
using Gameplay.FloatingDamage.Systems;
using Gameplay.HealthBars.Systems;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class HealthBarsModule : IWorldModule
    {
        public int Priority => 110;

        public void Initialize(World world)
        {
            var sg_HealthBarsRenders = world.CreateSystemsGroup();
            var sg_HealthBarsManage = world.CreateSystemsGroup();

            sg_HealthBarsRenders.AddSystem(new CellsHealthBarsSystem());
            //sg_HealthBarsRenders.AddSystem(new HealthBarsUpdateValueSystem());

            //sg_HealthBarsRenders.AddSystem(new FloatingDamage());

            sg_HealthBarsManage.AddSystem(new HealthBarManageSystem());

            world.AddSystemsGroup(Priority, sg_HealthBarsRenders);
            world.AddSystemsGroup(Priority + 1, sg_HealthBarsManage);
        }
    }
}


