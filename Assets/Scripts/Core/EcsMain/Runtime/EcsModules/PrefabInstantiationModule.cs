using Core.Utilities.Systems;
using Domain.ECS;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class PrefabInstantiationModule : IWorldModule
    {
        public int Priority => 200;

        public void Initialize(World world)
        {
            var sg_Prefabs = world.CreateSystemsGroup();
            sg_Prefabs.AddSystem(new EntityPrefabInstantiateSystem());

            world.AddSystemsGroup(Priority, sg_Prefabs);
        }
    }
}


