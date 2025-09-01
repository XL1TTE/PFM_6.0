using Core.Utilities.Systems;
using Domain.ECS;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
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


