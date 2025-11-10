using CursorDetection.Systems;
using Domain.ECS;
using Gameplay.Map.Systems;
using Scellecs.Morpeh;


namespace Core.ECS.Modules
{
    public sealed class MapInteractionBaseModule : IWorldModule
    {
        public int Priority => 250;

        public void Initialize(World world)
        {
            var sg_MapBase = world.CreateSystemsGroup();
            sg_MapBase.AddSystem(new CursorDetectionSystem());
            sg_MapBase.AddSystem(new MapClickObserveSystem());
            //sg_MapBase.AddSystem(new MapTextEventHandlerSystem());
            //sg_MapBase.AddSystem(new MapDrawSystem());
            sg_MapBase.AddSystem(new MapEvReqSystemUpdateProgress());

            world.AddSystemsGroup(Priority, sg_MapBase);
        }
    }
}
