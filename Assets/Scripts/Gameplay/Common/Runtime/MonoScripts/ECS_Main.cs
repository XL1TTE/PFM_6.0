using Gameplay.Common.Systems;
using Gameplay.Features.BattleField.Systems;
using Gameplay.Features.DragAndDrop.Systems;
using Scellecs.Morpeh;
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
            SystemsGroup BattleStageHandlerSystemGroup = _defaultWorld.CreateSystemsGroup();
            BattleStageHandlerSystemGroup.AddSystem(new PlanningStageHandleSystem());

            SystemsGroup MonsterSpawnSystemGroup = _defaultWorld.CreateSystemsGroup();
            MonsterSpawnSystemGroup.AddSystem(new MonstersSpawnRequestSystem());
            MonsterSpawnSystemGroup.AddSystem(new MonsterSpawnSystem());

            SystemsGroup DragAndDropSystemGroup = _defaultWorld.CreateSystemsGroup();
            DragAndDropSystemGroup.AddSystem(new CursorDetectionSystem());
            DragAndDropSystemGroup.AddSystem(new DragInputSystem());
            DragAndDropSystemGroup.AddSystem(new DragStartSystem());
            DragAndDropSystemGroup.AddSystem(new DragProcessSystem());
            DragAndDropSystemGroup.AddSystem(new DragEndSystem());

            SystemsGroup DragAndDropEventsHandlers = _defaultWorld.CreateSystemsGroup();
            DragAndDropEventsHandlers.AddSystem(new MonsterSpawnCellDropValidataionSystem());
            DragAndDropEventsHandlers.AddSystem(new DragAndDropCleanupSystem());

            SystemsGroup CellsSystemGroup = _defaultWorld.CreateSystemsGroup();
            CellsSystemGroup.AddSystem(new CellOccupySystem());
            CellsSystemGroup.AddSystem(new HighlightSpawnCellSystem());
            
            SystemsGroup MarkSystems = _defaultWorld.CreateSystemsGroup();
            MarkSystems.AddSystem(new DropTargetMarkSystem());

            _defaultWorld.AddSystemsGroup(0, BattleStageHandlerSystemGroup);
            _defaultWorld.AddSystemsGroup(1, MonsterSpawnSystemGroup);
            _defaultWorld.AddSystemsGroup(2, DragAndDropSystemGroup);
            _defaultWorld.AddSystemsGroup(3, DragAndDropEventsHandlers);
            _defaultWorld.AddSystemsGroup(4, CellsSystemGroup);
            _defaultWorld.AddSystemsGroup(5, MarkSystems);
        }


        void Update()
        {
            _defaultWorld.Update(Time.deltaTime);
            _defaultWorld.CleanupUpdate(Time.deltaTime);
            _defaultWorld.Commit();
        }

    }

}
