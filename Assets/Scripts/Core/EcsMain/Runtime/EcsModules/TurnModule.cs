using Domain.ECS;
using Gameplay.TurnSystem.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules
{
    public sealed class TurnModule : IWorldModule
    {
        public int Priority => -600;

        public void Initialize(World world)
        {
            var sg_TurnLogic = world.CreateSystemsGroup();
            //sg_TurnLogic.AddSystem(new TurnSystemInitializer());
            //sg_TurnLogic.AddSystem(new TurnProcessorSystem());

            world.AddSystemsGroup(Priority, sg_TurnLogic);
        }
    }
}


