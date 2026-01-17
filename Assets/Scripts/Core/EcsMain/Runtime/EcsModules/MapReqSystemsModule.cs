using Domain.Ecs;
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
            sg_MapReqs.AddSystem(new MapEvReqSystemGiveParts());
            sg_MapReqs.AddSystem(new MapEvReqSystemSwapParts());
            sg_MapReqs.AddSystem(new MapEvReqSystemSwapPartsBetween());
            sg_MapReqs.AddSystem(new MapEvReqSystemGiveHP());
            sg_MapReqs.AddSystem(new MapEvReqSystemTakeHP());


            world.AddSystemsGroup(Priority, sg_MapReqs);
        }
    }
}
