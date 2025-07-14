using ECS.Systems;
using Project.Domain;
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
        SystemsGroup LogFuture = _defaultWorld.CreateSystemsGroup();
        LogFuture.AddSystem(new CellPositionLogSystem(10));

        _defaultWorld.AddSystemsGroup(0, LogFuture);
    }

    
    void Update()
    {
        _defaultWorld.Update(Time.deltaTime);
        _defaultWorld.Commit();
    }
    
}
