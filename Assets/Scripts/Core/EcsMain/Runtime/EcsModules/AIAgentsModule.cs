using Domain.ECS;
using Gameplay.Enemies;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class AIAgentsModule : IWorldModule
    {
        public int Priority => -650;

        public void Initialize(World world)
        {
            var sg_AiAgents = world.CreateSystemsGroup();
            sg_AiAgents.AddSystem(new DefaultAIAgent());

            world.AddSystemsGroup(Priority, sg_AiAgents);
        }
    }
}


