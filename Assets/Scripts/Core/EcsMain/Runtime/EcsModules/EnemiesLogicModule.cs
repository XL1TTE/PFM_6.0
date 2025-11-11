using Domain.Ecs;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class EnemiesLogicModule : IWorldModule
    {
        public int Priority => -100;

        public void Initialize(World world)
        {
            var sg_EnemiesLogic = world.CreateSystemsGroup();
            //sg_EnemiesLogic.AddSystem(new EnemiesSpawnSystem());

            world.AddSystemsGroup(Priority, sg_EnemiesLogic);
        }
    }
}


