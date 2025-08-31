using Domain.ECS;
using Gameplay.DragAndDrop.Systems;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;
using UI.Systems;

namespace Core.ECS.Modules{
    public sealed class CleanupModule : IWorldModule
    {
        public int Priority => 300;

        public void Initialize(World world)
        {
            var sg_Cleanup = world.CreateSystemsGroup();
            sg_Cleanup.AddSystem(new DragAndDropCleanupSystem());
            sg_Cleanup.AddSystem(new StateExitCleanupSystem());
            sg_Cleanup.AddSystem(new FpsShowSystem());

            world.AddSystemsGroup(Priority, sg_Cleanup);
        }
    }
}


