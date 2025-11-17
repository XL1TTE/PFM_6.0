using CursorDetection.Systems;
using Domain.Ecs;
using Gameplay.Map.Systems;
using Scellecs.Morpeh;


namespace Core.Ecs.Modules
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

            world.AddSystemsGroup(Priority, sg_MapBase);
        }
    }
}
