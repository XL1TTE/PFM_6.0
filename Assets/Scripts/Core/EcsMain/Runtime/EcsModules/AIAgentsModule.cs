using Domain.ECS;
using Gameplay.AIGraph;
using Gameplay.Enemies;
using Scellecs.Morpeh;

namespace Core.ECS.Modules
{
    public sealed class AIAgentsModule : IWorldModule
    {
        public int Priority => -650;

        public void Initialize(World world)
        {
            var sg_AiAgents = world.CreateSystemsGroup();
            var sg_AgentsMove = world.CreateSystemsGroup();
            var sg_AgentsTargetSelection = world.CreateSystemsGroup();

            sg_AiAgents.AddSystem(new AiActivationSystem());
            sg_AiAgents.AddSystem(new AiGraphExecutionSystem());
            sg_AiAgents.AddSystem(new AiAgentStateUpdateSystem());

            //sg_AgentsMove.AddSystem(new DefaultAgentMoveSystem());

            sg_AgentsTargetSelection.AddSystem(new DefaultAgentTargetSelectSystem());



            world.AddSystemsGroup(Priority, sg_AiAgents);
            world.AddSystemsGroup(Priority + 1, sg_AgentsMove);
            world.AddSystemsGroup(Priority + 2, sg_AgentsTargetSelection);
        }
    }
}


