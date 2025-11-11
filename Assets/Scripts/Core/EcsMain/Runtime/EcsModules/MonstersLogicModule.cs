using Domain.Ecs;
using Gameplay.Monster.Systems;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class MonstersLogicModule : IWorldModule
    {
        public int Priority => -200;

        public void Initialize(World world)
        {
            var sg_MonsterLogic = world.CreateSystemsGroup();
            sg_MonsterLogic.AddSystem(new MonsterSpawnSystem());
            sg_MonsterLogic.AddSystem(new MonsterGhostSystem());
            sg_MonsterLogic.AddSystem(new MonsterActionsControlSystem());

            world.AddSystemsGroup(Priority, sg_MonsterLogic);
        }
    }
}


