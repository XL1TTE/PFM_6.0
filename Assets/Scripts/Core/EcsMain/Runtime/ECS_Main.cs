
using Codice.CM.WorkspaceServer.DataStore.WkTree;
using Core.Utilities.Systems;
using CursorDetection.Systems;
using Domain.StateMachine.Mono;
using Gameplay.Abilities.Systems;
using Gameplay.BattleField.Systems;
using Gameplay.DragAndDrop.Systems;
using Gameplay.DragAndDrop.Validators;
using Gameplay.EcsButtons.Systems;
using Gameplay.Enemies;
using Gameplay.Monster.Systems;
using Gameplay.StateMachine.Systems;
using Gameplay.TargetSelection.Systems;
using Gameplay.TurnSystem.Systems;
using Persistence.DB;
using Scellecs.Morpeh;
using UI.Systems;
using UnityEngine;

namespace Gameplay.Common.Mono{
    public class ECS_Main : MonoBehaviour
    {
        public static World _defaultWorld;
                
        void Awake()
        {
            _defaultWorld = World.Default;

            _defaultWorld.UpdateByUnity = false;
        }

        void Start()
        {
            ConfigureSystems();
            DataBase.Initialize();
        }

        private void ConfigureSystems()
        {
            SystemsGroup SharedUISystemGroup = _defaultWorld.CreateSystemsGroup();
            SharedUISystemGroup.AddInitializer(new SharedUI_Initializer());
            SharedUISystemGroup.AddSystem(new FullScreenNotificationSystem());
            
            SystemsGroup UIElements = _defaultWorld.CreateSystemsGroup();
            UIElements.AddSystem(new FpsShowSystem());

            SystemsGroup StateMachineSystemGroup = _defaultWorld.CreateSystemsGroup();
            //StateMachineSystemGroup.AddInitializer(new StateMachineInitializer());
            //StateMachineSystemGroup.AddSystem(new StateMachineSystem());
            //StateMachineSystemGroup.AddSystem(new StateTransitionSystem());

            SystemsGroup BattlePlanningState = _defaultWorld.CreateSystemsGroup();
            BattlePlanningState.AddInitializer(new BattlePlanningStateInitializer());
            BattlePlanningState.AddSystem(new BattlePlanningStateInitializeEnterSystem());
            BattlePlanningState.AddSystem(new BattlePlanningInitializeExitSystem());
            
            // State handle systems
            SystemsGroup InBattlePlanningStateSystems = _defaultWorld.CreateSystemsGroup();
            InBattlePlanningStateSystems.AddSystem(new MonsterSpawnCellMonsterDragInPlanningState());
            
            SystemsGroup BattleState = _defaultWorld.CreateSystemsGroup();
            BattleState.AddInitializer(new BattleStateInitializer());
            BattleState.AddSystem(new BattleInitializeEnterSystem());
            
            SystemsGroup TurnSystem = _defaultWorld.CreateSystemsGroup();
            TurnSystem.AddSystem(new TurnSystemInitializer());
            TurnSystem.AddSystem(new TurnProcessorSystem());

            SystemsGroup TurnSystemReactors = _defaultWorld.CreateSystemsGroup();
            TurnSystemReactors.AddSystem(new TurnQueueRenderSystem());
            TurnSystemReactors.AddSystem(new MonsterAvatarDrawSystem());
            TurnSystemReactors.AddSystem(new MonsterAbilitiesDrawSystem());
            TurnSystemReactors.AddSystem(new TurnTakerCellMarkSystem());

            SystemsGroup MonsterSystems = _defaultWorld.CreateSystemsGroup();
            MonsterSystems.AddSystem(new MonsterSpawnSystem());
            MonsterSystems.AddSystem(new MonsterGhostSystem());
            
            SystemsGroup AbilitySystem = _defaultWorld.CreateSystemsGroup();
            AbilitySystem.AddSystem(new MoveAbilitySystem());

            SystemsGroup DragAndDropSystemGroup = _defaultWorld.CreateSystemsGroup();
            DragAndDropSystemGroup.AddSystem(new CursorDetectionSystem());
            DragAndDropSystemGroup.AddSystem(new DragInputSystem());
            DragAndDropSystemGroup.AddSystem(new DragStartSystem());
            DragAndDropSystemGroup.AddSystem(new DragProcessSystem());
            DragAndDropSystemGroup.AddSystem(new DragEndSystem());

            SystemsGroup DragAndDropEventsHandlers = _defaultWorld.CreateSystemsGroup();
            DragAndDropEventsHandlers.AddSystem(new MonsterSpawnCellDropValidataionSystem());
            DragAndDropEventsHandlers.AddSystem(new NotHandledDropProcessSystem());
            DragAndDropEventsHandlers.AddSystem(new DragAndDropCleanupSystem());
            
            SystemsGroup TargetSelection = _defaultWorld.CreateSystemsGroup();
            TargetSelection.AddSystem(new TargetSelectionSystem());

            SystemsGroup CellsSystemGroup = _defaultWorld.CreateSystemsGroup();
            CellsSystemGroup.AddSystem(new CellOccupySystem());
            CellsSystemGroup.AddSystem(new CellHoverSystem());
            CellsSystemGroup.AddSystem(new CellsViewSystem());
            
            SystemsGroup MarkSystems = _defaultWorld.CreateSystemsGroup();
            MarkSystems.AddSystem(new DropTargetMarkSystem());
            MarkSystems.AddSystem(new MonsterDragControlSystem());
            
            SystemsGroup ButtonSystems = _defaultWorld.CreateSystemsGroup();
            ButtonSystems.AddSystem(new ButtonClickObserveSystem());
            ButtonSystems.AddSystem(new ExitPlanningStageButtonSystem());
            ButtonSystems.AddSystem(new NextTurnButtonSystem());

            SystemsGroup EnemiesSystems = _defaultWorld.CreateSystemsGroup();
            EnemiesSystems.AddSystem(new EnemiesSpawnSystem());
            
            
            SystemsGroup UtilitySystems = _defaultWorld.CreateSystemsGroup();
            UtilitySystems.AddSystem(new EntityPrefabInstantiateSystem());

            SystemsGroup CleanupSystems = _defaultWorld.CreateSystemsGroup();
            CleanupSystems.AddSystem(new StateExitCleanupSystem());


            _defaultWorld.AddSystemsGroup(0, SharedUISystemGroup);
            _defaultWorld.AddSystemsGroup(1, StateMachineSystemGroup);
            _defaultWorld.AddSystemsGroup(2, InBattlePlanningStateSystems);
            _defaultWorld.AddSystemsGroup(4, ButtonSystems);
            _defaultWorld.AddSystemsGroup(5, UIElements);
            _defaultWorld.AddSystemsGroup(6, DragAndDropSystemGroup);
            _defaultWorld.AddSystemsGroup(7, MonsterSystems);
            _defaultWorld.AddSystemsGroup(8, EnemiesSystems);
            _defaultWorld.AddSystemsGroup(9, AbilitySystem);
            _defaultWorld.AddSystemsGroup(10, TurnSystem);
            _defaultWorld.AddSystemsGroup(11, TurnSystemReactors);
            _defaultWorld.AddSystemsGroup(12, TargetSelection);
            _defaultWorld.AddSystemsGroup(13, DragAndDropEventsHandlers);
            _defaultWorld.AddSystemsGroup(14, CellsSystemGroup);
            _defaultWorld.AddSystemsGroup(15, MarkSystems);
            _defaultWorld.AddSystemsGroup(16, UtilitySystems);
            _defaultWorld.AddSystemsGroup(17, BattleState);
            _defaultWorld.AddSystemsGroup(18, BattlePlanningState);
            _defaultWorld.AddSystemsGroup(19, CleanupSystems);
        }


    void Update()
        {
            _defaultWorld.Update(Time.deltaTime);
            _defaultWorld.CleanupUpdate(Time.deltaTime);
            _defaultWorld.Commit();
            
            StateMachineWorld.Update();
        }

    }

}
