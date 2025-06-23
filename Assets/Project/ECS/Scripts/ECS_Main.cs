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
    }

    
    void Update()
    {
        _defaultWorld.Update(Time.deltaTime);
    }
    
}
