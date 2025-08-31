using Domain.ECS;
using Gameplay.Enemies;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class EnemiesLogicModule : IWorldModule
    {
        public int Priority => -200;

        public void Initialize(World world)
        {
            var sg_EnemiesLogic = world.CreateSystemsGroup();
            sg_EnemiesLogic.AddSystem(new EnemiesSpawnSystem());

            world.AddSystemsGroup(Priority, sg_EnemiesLogic);
        }
    }
}


