using CursorDetection.Systems;
using Domain.ECS;
using Gameplay.Map.Systems;
using Gameplay.MapEvents.Systems;
using Scellecs.Morpeh;


namespace Core.ECS.Modules
{
    public sealed class MapReqSystemsModule : IWorldModule
    {
        public int Priority => 300;

        public void Initialize(World world)
        {
            var sg_MapReqs = world.CreateSystemsGroup();
            sg_MapReqs.AddSystem(new MapEvReqSystemGiveGold());
            sg_MapReqs.AddSystem(new MapEvReqSystemTakeGold());


            world.AddSystemsGroup(Priority, sg_MapReqs);
        }
    }
}
