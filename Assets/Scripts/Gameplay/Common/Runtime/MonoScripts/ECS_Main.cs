using Core.Systems;
using Gameplay.Common.Systems;
using Gameplay.Features.Abilities;
using Gameplay.Features.BattleField.Systems;
using Gameplay.Features.DragAndDrop.Systems;
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

            SystemsGroup StateMachineSystemGroup = _defaultWorld.CreateSystemsGroup();
            StateMachineSystemGroup.AddInitializer(new StateMachineInitializer());
            StateMachineSystemGroup.AddSystem(new StateMachineSystem());
            StateMachineSystemGroup.AddSystem(new StateTransitionSystem());

            SystemsGroup BattlePlanningState = _defaultWorld.CreateSystemsGroup();
            BattlePlanningState.AddInitializer(new BattlePlanningStateInitializer());
            BattlePlanningState.AddSystem(new BattlePlanningStateEnterSystem());
            BattlePlanningState.AddSystem(new BattlePlanningStateExitSystem());
            
            SystemsGroup BattleState = _defaultWorld.CreateSystemsGroup();
            BattleState.AddInitializer(new BattleStateInitializer());
            BattleState.AddSystem(new BattleStateEnterSystem());
            
            SystemsGroup TurnSystem = _defaultWorld.CreateSystemsGroup();
            TurnSystem.AddSystem(new TurnSystemInitializer());
 
            SystemsGroup MonsterSystems = _defaultWorld.CreateSystemsGroup();
            MonsterSystems.AddSystem(new MonstersSpawnRequestSystem());
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
            CellsSystemGroup.AddSystem(new HighlightSpawnCellSystem());
            CellsSystemGroup.AddSystem(new CellsViewSystem());
            
            SystemsGroup MarkSystems = _defaultWorld.CreateSystemsGroup();
            MarkSystems.AddSystem(new DropTargetMarkSystem());
            MarkSystems.AddSystem(new MonsterDragControlSystem());
            
            SystemsGroup ButtonSystems = _defaultWorld.CreateSystemsGroup();
            ButtonSystems.AddSystem(new ButtonClickObserveSystem());
            ButtonSystems.AddSystem(new ExitPlanningStageButtonSystem());

            _defaultWorld.AddSystemsGroup(0, SharedUISystemGroup);
            _defaultWorld.AddSystemsGroup(1, StateMachineSystemGroup);
            _defaultWorld.AddSystemsGroup(2, BattlePlanningState);
            _defaultWorld.AddSystemsGroup(3, BattleState);
            _defaultWorld.AddSystemsGroup(4, TurnSystem);
            _defaultWorld.AddSystemsGroup(5, ButtonSystems);
            _defaultWorld.AddSystemsGroup(6, UIElements);
            _defaultWorld.AddSystemsGroup(7, DragAndDropSystemGroup);
            _defaultWorld.AddSystemsGroup(8, MonsterSystems);
            _defaultWorld.AddSystemsGroup(9, AbilitySystem);
            _defaultWorld.AddSystemsGroup(10, TargetSelection);
            _defaultWorld.AddSystemsGroup(11, DragAndDropEventsHandlers);
            _defaultWorld.AddSystemsGroup(12, CellsSystemGroup);
            _defaultWorld.AddSystemsGroup(13, MarkSystems);
        }


        void Update()
        {
            _defaultWorld.Update(Time.deltaTime);
            _defaultWorld.CleanupUpdate(Time.deltaTime);
            _defaultWorld.Commit();
        }

    }

}
