using Domain.ECS;
using Gameplay.DragAndDrop.Systems;
using Gameplay.DragAndDrop.Validators;
using Gameplay.Monster.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class DragAndDropLogicModule : IWorldModule
    {
        public int Priority => -400;

        public void Initialize(World world)
        {
            var sg_DragLogic = world.CreateSystemsGroup();
            sg_DragLogic.AddSystem(new DragStartSystem());
            sg_DragLogic.AddSystem(new DragProcessSystem());
            sg_DragLogic.AddSystem(new DragEndSystem());
            sg_DragLogic.AddSystem(new MonsterDragControlSystem());
            sg_DragLogic.AddSystem(new DropTargetMarkSystem());



            var sg_DragHandleLogic = world.CreateSystemsGroup();

            sg_DragHandleLogic.AddSystem(new MonsterSpawnCellDropValidataionSystem());
            sg_DragHandleLogic.AddSystem(new NotHandledDropProcessSystem());

            world.AddSystemsGroup(Priority, sg_DragLogic);
            world.AddSystemsGroup(Priority + 1, sg_DragHandleLogic);
        }
    }
}


