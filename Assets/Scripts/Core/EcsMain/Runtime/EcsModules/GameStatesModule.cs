using Domain.ECS;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class GameStatesModule : IWorldModule
    {
        public int Priority => -800;

        public void Initialize(World world)
        {
            var sg_BattlePlanningState = world.CreateSystemsGroup();
            sg_BattlePlanningState.AddInitializer(new BattlePlanningStateInitializer());
            sg_BattlePlanningState.AddSystem(new BattlePlanningStateInitializeEnterSystem());
            sg_BattlePlanningState.AddSystem(new BattlePlanningInitializeExitSystem());

            var sg_BattleState = world.CreateSystemsGroup();
            sg_BattleState.AddInitializer(new BattleStateInitializer());
            sg_BattleState.AddSystem(new BattleInitializeEnterSystem());

            var sg_StateHandlers = world.CreateSystemsGroup();
            sg_StateHandlers.AddSystem(new MonsterSpawnCellMonsterDragInPlanningState());

            world.AddSystemsGroup(Priority, sg_BattlePlanningState);
            world.AddSystemsGroup(Priority + 1, sg_BattleState);
            world.AddSystemsGroup(Priority + 2, sg_StateHandlers);
        }
    }
}


