using ECS.Components;
using ECS.Components.Monsters;
using ECS.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class CellOccupySystem : ISystem 
{
    public World World { get; set;}
 
    private Filter _occupiedCells;
    
    private Event<CellOccupiedEvent> _cellOccupiedEvent;    
    private Stash<TagOccupiedCell> stash_occupiedCell;

    public void OnAwake() 
    {
        _occupiedCells = World.Filter
            .With<TagOccupiedCell>()
            .Build();

        _cellOccupiedEvent = World.GetEvent<CellOccupiedEvent>();

        stash_occupiedCell = World.GetStash<TagOccupiedCell>();
    }

    public void OnUpdate(float deltaTime) 
    {  
        foreach (var evt in _cellOccupiedEvent.publishedChanges)
        {     
            // Unoccupy previous cell
            foreach(var cell in _occupiedCells)
            {
                if(stash_occupiedCell.Get(cell).Occupier.Id == evt.OccupiedBy.Id){
                    stash_occupiedCell.Remove(cell);
                }
            }
            stash_occupiedCell.Set(evt.CellEntity, new TagOccupiedCell
            {
                Occupier = evt.OccupiedBy
            });
        }

    }

    public void Dispose()
    {

    }
}
