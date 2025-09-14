using Domain.ECS;
using Gameplay.AbilityGraph;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class NewAbilitiesModule : IWorldModule
    {
        public int Priority => -50;

        public void Initialize(World world)
        {
            var sg_AbilitiesLogic = world.CreateSystemsGroup();
            sg_AbilitiesLogic.AddSystem(new AbilityActivationSystem());
            sg_AbilitiesLogic.AddSystem(new AbilityGraphExecutionSystem());
            sg_AbilitiesLogic.AddSystem(new AbilityExecutionStateUpdateSystem());
            sg_AbilitiesLogic.AddSystem(new AbilityGraphExecutionCleanup());

            world.AddSystemsGroup(Priority, sg_AbilitiesLogic);
        }
    }
}


