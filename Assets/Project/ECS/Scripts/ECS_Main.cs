using ECS.Systems;
using Scellecs.Morpeh;
using UnityEngine;

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
        
        SystemsGroup MonsterFitures = _defaultWorld.CreateSystemsGroup();
        MonsterFitures.AddSystem(new MonsterDragHandleSystem());

        SystemsGroup CellsSystemGroup = _defaultWorld.CreateSystemsGroup();
        CellsSystemGroup.AddSystem(new CellOccupySystem());
        CellsSystemGroup.AddSystem(new HighlightSpawnCellSystem());

        _defaultWorld.AddSystemsGroup(0, BattleStageHandlerSystemGroup);
        _defaultWorld.AddSystemsGroup(1, MonsterSpawnSystemGroup);
        _defaultWorld.AddSystemsGroup(2, MonsterFitures);
        _defaultWorld.AddSystemsGroup(3, CellsSystemGroup);
    }

    
    void Update()
    {
        _defaultWorld.Update(Time.deltaTime);
        _defaultWorld.Commit();
    }
    
}
