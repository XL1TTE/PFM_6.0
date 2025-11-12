using Domain.Ecs;
using Gameplay.Map.Systems;
using Scellecs.Morpeh;


namespace Core.ECS.Modules
{
    public sealed class MapNodeWaitModule : IWorldModule
    {
        public int Priority => 200;

        public void Initialize(World world)
        {
            var sg_NodeWaits = world.CreateSystemsGroup();
            sg_NodeWaits.AddSystem(new MapNodeClickTextWaitSystem());
            sg_NodeWaits.AddSystem(new MapNodeClickBattleWaitSystem());
            sg_NodeWaits.AddSystem(new MapNodeClickBossWaitSystem());
            sg_NodeWaits.AddSystem(new MapNodeClickLabWaitSystem());

            world.AddSystemsGroup(Priority, sg_NodeWaits);
        }
    }
}
