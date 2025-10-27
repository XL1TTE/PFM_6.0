using Domain.ECS;
using Gameplay.Commands;
using Scellecs.Morpeh;

namespace Core.ECS.Modules
{
    public sealed class ServicesModule : IWorldModule
    {
        public int Priority => 50;

        public void Initialize(World world)
        {
            var sg_Commands = world.CreateSystemsGroup();
            sg_Commands.AddSystem(new ActorsActionStateInitializer());
            sg_Commands.AddSystem(new AnimationSeviceSystem());
            sg_Commands.AddSystem(new MovementServiceSystem());
            //sg_Commands.AddSystem(new DamageSeviceSystem());

            sg_Commands.AddSystem(new AnimationsObserver());
            sg_Commands.AddSystem(new AbilitiesObserver());

            world.AddSystemsGroup(Priority, sg_Commands);
        }
    }
}


