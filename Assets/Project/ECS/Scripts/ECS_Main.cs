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
        /* ############################################## */
        /*                   Move Future                  */
        /* ############################################## */
        SystemsGroup MoveFuture = _defaultWorld.CreateSystemsGroup();

        MoveFuture.AddSystem(new MoveInputObserveSystem());
        MoveFuture.AddSystem(new MoveSystem());
        
        _defaultWorld.AddSystemsGroup(0, MoveFuture);
    }

    
    void Update()
    {
        _defaultWorld.Update(Time.deltaTime);
    }
    
}
