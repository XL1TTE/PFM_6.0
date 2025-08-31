using Domain.ECS;
using Gameplay.Monster.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class MonstersLogicModule : IWorldModule
    {
        public int Priority => -300;

        public void Initialize(World world)
        {
            var sg_MonsterLogic = world.CreateSystemsGroup();
            sg_MonsterLogic.AddSystem(new MonsterSpawnSystem());
            sg_MonsterLogic.AddSystem(new MonsterGhostSystem());

            world.AddSystemsGroup(Priority, sg_MonsterLogic);
        }
    }
}


