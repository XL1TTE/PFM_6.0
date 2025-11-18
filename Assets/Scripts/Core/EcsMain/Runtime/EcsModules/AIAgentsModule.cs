using Domain.Ecs;
using Gameplay.AIGraph;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class AIAgentsModule : IWorldModule
    {
        public int Priority => -650;

        public void Initialize(World world)
        {
            var sg_AiAgents = world.CreateSystemsGroup();

            //sg_AiAgents.AddSystem(new AiActivationSystem());

            world.AddSystemsGroup(Priority, sg_AiAgents);
        }
    }
}


