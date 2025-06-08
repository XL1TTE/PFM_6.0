using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class MoveSystem : ISystem 
{
    public World World { get; set;}

    private Filter _filter;

    private Stash<MoveInputComponent> _miStash;
    private Stash<UnityTransformComponent> _utStash;
    private Stash<MovementConfigComponent> _mcStash;

    public void OnAwake() 
    {
        _filter = World.Filter
            .With<MoveInputComponent>()
            .With<UnityTransformComponent>()
            .Build();

        _miStash = World.GetStash<MoveInputComponent>();
        _utStash = World.GetStash<UnityTransformComponent>();
        _mcStash = World.GetStash<MovementConfigComponent>();
    }
    
    public void OnUpdate(float deltaTime) 
    {
        foreach(var e in _filter){
            var moveInput = _miStash.Get(e);
            var tComp = _utStash.Get(e);
            
            var defaultSpeed = 1.0f;
            if(_mcStash.Has(e)){
                defaultSpeed = _mcStash.Get(e).speed;
            }
            
            tComp.transform.position += 
                new UnityEngine.Vector3(
                    moveInput.horizontal * defaultSpeed * deltaTime, 
                    moveInput.vertical * defaultSpeed * deltaTime);
        }
    }

    public void Dispose()
    {

    }
}
