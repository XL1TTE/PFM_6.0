using System.Collections.Generic;
using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class CellHoverSystem : ISystem 
{
    public World World { get; set;}
    
    private Filter _cellsUnderCursor; 
    
    private Request<CellSpriteChangeRequest> req_cellSpriteChange;
    private Stash<CellSpritesComponent> stash_cellSprites;
    
    private Entity CurrentHighlightedCell;

    public void OnAwake() 
    {
        _cellsUnderCursor = World.Filter
            .With<UnderCursorComponent>()
            .With<CellSpritesComponent>()
            .With<CellTag>()
            .Build();

        req_cellSpriteChange = World.GetRequest<CellSpriteChangeRequest>();

        stash_cellSprites = World.GetStash<CellSpritesComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        if(_cellsUnderCursor.IsEmpty() && CurrentHighlightedCell.IsExist())
        {
            DisableHighlight();
            return;
        }
        
        var cell = _cellsUnderCursor.FirstOrDefault();
        if(cell.IsExist()){
              
            var state = stash_cellSprites.Get(cell).SpriteState;
            if (state != CellSpritesComponent.SpriteStates.Hovered){
                if(CurrentHighlightedCell.Id != cell.Id){
                    DisableHighlight();
                }
                EnableHighlight(cell);
            }
        }
    }

    public void Dispose()
    {

    }
    
    private void DisableHighlight(){
        // Disable higlighting for previous cell.
        if (CurrentHighlightedCell.IsExist())
        {
            req_cellSpriteChange.Publish(new CellSpriteChangeRequest
            {
                Cells = new List<Entity> { CurrentHighlightedCell },
                Sprite = CellSpriteChangeRequest.SpriteType.Previous
            });

            CurrentHighlightedCell = default;
        }
    }
    
    private void EnableHighlight(Entity cell){
        CurrentHighlightedCell = cell;
        req_cellSpriteChange.Publish(new CellSpriteChangeRequest
        {
            Cells = new List<Entity> { cell },
            Sprite = CellSpriteChangeRequest.SpriteType.Hover
        });
    }
}
