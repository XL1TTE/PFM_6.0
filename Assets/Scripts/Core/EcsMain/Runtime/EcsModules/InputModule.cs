using CursorDetection.Systems;
using Domain.ECS;
using Gameplay.DragAndDrop.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class InputModule : IWorldModule
    {
        public int Priority => -500;

        public void Initialize(World world)
        {
            var sg_Input = world.CreateSystemsGroup();
            sg_Input.AddSystem(new CursorDetectionSystem());
            sg_Input.AddSystem(new DragInputSystem());
            sg_Input.AddSystem(new ButtonClickObserveSystem());

            world.AddSystemsGroup(Priority, sg_Input);
        }
    }
}


