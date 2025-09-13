using Domain.ECS;
using Gameplay.Commands;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class CommandsModule : IWorldModule
    {
        public int Priority => 50;

        public void Initialize(World world)
        {
            var sg_Commands= world.CreateSystemsGroup();
            sg_Commands.AddSystem(new MoveCommandSystem());

            world.AddSystemsGroup(Priority, sg_Commands);
        }
    }
}


