
using Scellecs.Morpeh;
using UnityEngine;

public class MoveInputObserveSystem : ISystem
{
    public World World { get; set; }

    private Filter _filter;
    
    private Stash<MoveInputComponent> _miStash;
    
    public void Dispose(){}

    public void OnAwake()
    {
        _filter = World.Filter
            .With<MoveInputComponent>()
            .With<ControlableComponent>()
            .Build();

        _miStash = World.GetStash<MoveInputComponent>();
    }
    
    /// <summary>
    /// Writing move input in MoveInputComponent of entities with ControlableComponent
    /// </summary>
    /// <param name="deltaTime"></param>
    public void OnUpdate(float deltaTime)
    {
        if(_filter.IsEmpty()) {return;}

        var ver = Input.GetAxis("Vertical");
        var hor = Input.GetAxis("Horizontal");

        foreach(var e in _filter){
            ref var miComp = ref _miStash.Get(e);
            
            miComp.horizontal = hor;
            miComp.vertical = ver;
        }
    }
}
