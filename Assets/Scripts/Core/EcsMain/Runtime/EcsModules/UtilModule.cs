using Core.Utilities.Systems;
using Domain.Ecs;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
    public sealed class UtilModule : IWorldModule
    {
        public int Priority => 99999;

        public void Initialize(World world)
        {
            var sg_Util = world.CreateSystemsGroup();
            sg_Util.AddSystem(new EntityPrefabInstantiateSystem());
            sg_Util.AddSystem(new StateExitCleanupSystem());

            world.AddSystemsGroup(Priority, sg_Util);
        }
    }
}


