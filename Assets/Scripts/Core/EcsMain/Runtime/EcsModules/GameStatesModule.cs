using Domain.ECS;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class GameStatesModule : IWorldModule
    {
        public int Priority => -800;

        public void Initialize(World world)
        {
            var sq_BattleSceneInitialize = world.CreateSystemsGroup();
            sq_BattleSceneInitialize.AddInitializer(new BattleSceneStatesEntrypoint());
            sq_BattleSceneInitialize.AddSystem(new BattleSceneInitializeEnterSystem());
            sq_BattleSceneInitialize.AddSystem(new BattleSceneInitializeExitSystem());

            var sg_BattlePlanningState = world.CreateSystemsGroup();
            sg_BattlePlanningState.AddSystem(new PreBattlePlanningNotificationStateEnterSystem());
            sg_BattlePlanningState.AddSystem(new BattlePlanningStateEnterSystem());
            sg_BattlePlanningState.AddSystem(new BattlePlanningStateExitSystem());

            var sg_BattleState = world.CreateSystemsGroup();
            sg_BattleState.AddSystem(new PreBattleNotificationStateEnterSystem());
            sg_BattleState.AddSystem(new BattleStateEnterSystem());

            var sg_StateHandlers = world.CreateSystemsGroup();
            sg_StateHandlers.AddSystem(new MonsterSpawnCellMonsterDragInPlanningState());

            world.AddSystemsGroup(Priority, sq_BattleSceneInitialize);
            world.AddSystemsGroup(Priority + 1, sg_BattlePlanningState);
            world.AddSystemsGroup(Priority + 2, sg_BattleState);
            world.AddSystemsGroup(Priority + 3, sg_StateHandlers);
        }
    }
}


