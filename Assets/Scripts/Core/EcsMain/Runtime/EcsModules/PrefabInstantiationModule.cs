using Core.Utilities.Systems;
using Domain.Ecs;
using Scellecs.Morpeh;

namespace Core.Ecs.Modules
{
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


