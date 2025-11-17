using CursorDetection.Systems;
using Domain.Ecs;
using Gameplay.Map.Systems;
using Gameplay.MapEvents.Systems;
using Scellecs.Morpeh;


namespace Core.Ecs.Modules
{
    public sealed class MapReqSystemsModule : IWorldModule
    {
        public int Priority => 260;

        public void Initialize(World world)
        {
            var sg_MapReqs = world.CreateSystemsGroup();
            sg_MapReqs.AddSystem(new MapEvReqSystemGiveGold());
            sg_MapReqs.AddSystem(new MapEvReqSystemTakeGold());


            world.AddSystemsGroup(Priority, sg_MapReqs);
        }
    }
}
