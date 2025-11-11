using Domain.Ecs;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class CoreModule : IWorldModule
    {
        public int Priority => -1000;

        public void Initialize(World world)
        {
            var sg_Core = world.CreateSystemsGroup();

            world.AddSystemsGroup(Priority, sg_Core);
        }
    }
}


