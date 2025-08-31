using Domain.ECS;
using Gameplay.EcsButtons.Systems;
using Scellecs.Morpeh;
using UI.Systems;

namespace Core.ECS.Modules{
    public sealed class UILogicModule : IWorldModule
    {
        public int Priority => -700;

        public void Initialize(World world)
        {
            var sg_UILogic = world.CreateSystemsGroup();
            sg_UILogic.AddInitializer(new SharedUI_Initializer());
            sg_UILogic.AddSystem(new ExitPlanningStageButtonSystem());
            sg_UILogic.AddSystem(new NextTurnButtonSystem());

            world.AddSystemsGroup(Priority, sg_UILogic);
        }
    }
}


