using ECS.Components;
using ECS.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class CellOccupySystem : ISystem 
{
    public World World { get; set;}
    
    private Event<CellOccupiedEvent> _cellOccupiedEvent;
    
    private Stash<TagOccupiedCell> _occupiedCellStash;

    public void OnAwake() 
    {
        _cellOccupiedEvent = World.GetEvent<CellOccupiedEvent>();

        _occupiedCellStash = World.GetStash< TagOccupiedCell>();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach(var evt in _cellOccupiedEvent.publishedChanges){
            if(_occupiedCellStash.Has(evt.CellEntity)){continue;}
            _occupiedCellStash.Add(evt.CellEntity);
        }
    }

    public void Dispose()
    {

    }
}
