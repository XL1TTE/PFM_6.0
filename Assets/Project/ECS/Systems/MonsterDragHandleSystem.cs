using ECS.Components.Monsters;
using ECS.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class MonsterDragHandleSystem : ISystem 
{
    public World World { get; set;}

    private Request<EnableMonsterDragRequest> _monsterDragEnableRequest;
    private Request<DisableMonsterDragRequest> _monsterDragDisableRequest;

    private Filter _monstersWithDraggableComponent;   
    
    private Stash<DraggableComponent> _draggableStash;
    
    public void OnAwake() 
    {
        _monsterDragEnableRequest = World.GetRequest<EnableMonsterDragRequest>();
        _monsterDragDisableRequest = World.GetRequest<DisableMonsterDragRequest>();

        _monstersWithDraggableComponent = World.Filter
            .With<TagMonster>()
            .With<DraggableComponent>()
            .Build();


        _draggableStash = World.GetStash<DraggableComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        if(_monstersWithDraggableComponent.IsEmpty()){return;}
        
        // Enabling
        foreach(var req in _monsterDragEnableRequest.Consume()){
            foreach(var e in _monstersWithDraggableComponent){
                ref DraggableComponent drag = ref _draggableStash.Get(e);
                
                drag.Draggable.EnableDragBehaviour();
            }
        }
        //Disabling
        foreach(var req in _monsterDragDisableRequest.Consume()){
            foreach(var e in _monstersWithDraggableComponent){
                ref DraggableComponent drag = ref _draggableStash.Get(e);
                
                drag.Draggable.DisableDragBehaviour();
            }
        }
    }

    public void Dispose()
    {
        _monstersWithDraggableComponent.Dispose();
    }
}
