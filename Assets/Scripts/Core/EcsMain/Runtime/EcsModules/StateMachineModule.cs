using Domain.ECS;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class StateMachineModule : IWorldModule
    {
        public int Priority => -900;

        public void Initialize(World world)
        {
            var sg_StateMachine = world.CreateSystemsGroup();
            // sg_StateMachine.AddInitializer(new StateMachineInitializer());
            // sg_StateMachine.AddSystem(new StateMachineSystem());
            // sg_StateMachine.AddSystem(new StateTransitionSystem());

            world.AddSystemsGroup(Priority, sg_StateMachine);
        }
    }
}


