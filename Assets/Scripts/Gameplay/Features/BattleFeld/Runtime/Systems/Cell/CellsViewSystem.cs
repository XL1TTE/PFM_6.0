
using Domain.BattleField.Components;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.BattleField.Systems
{
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CellsViewSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<CellColorChangeRequest> req_ChangeColor;
        private Request<ChangeCellViewToHoverRequest> req_HoverCells;
        private Request<ChangeCellViewToSelectRequest> req_SelectCells;
        private Request<ChangeCellViewToPointerRequest> req_PointerCells;
        
        private Stash<CellTag> stash_cellTag;
        private Stash<SpriteComponent> stash_spriteRef;
        private Stash<CellSpriteLayersComponent> stash_cellSprites;

        public void OnAwake()
        {
            req_ChangeColor = World.GetRequest<CellColorChangeRequest>();
            req_HoverCells = World.GetRequest<ChangeCellViewToHoverRequest>();
            req_SelectCells = World.GetRequest<ChangeCellViewToSelectRequest>();
            req_PointerCells = World.GetRequest<ChangeCellViewToPointerRequest>();

            stash_cellTag = World.GetStash<CellTag>();
            stash_spriteRef = World.GetStash<SpriteComponent>();
            stash_cellSprites = World.GetStash<CellSpriteLayersComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_ChangeColor.Consume()){
                ChangeColor(req);
            }
            foreach(var req in req_HoverCells.Consume()){
                HoverCells(req);
            }
            foreach(var req in req_SelectCells.Consume()){
                SelectCells(req);
            }
            foreach(var req in req_PointerCells.Consume()){
                PointerCell(req);
            }
        }

        public void Dispose()
        {

        }


        private void PointerCell(ChangeCellViewToPointerRequest req)
        {
            foreach (var cell in req.Cells)
            {
                if(cell.IsExist() == false){continue;}
                if (stash_cellTag.Has(cell) == false) { continue; }
                if (stash_cellSprites.Has(cell) == false) { continue; }

                ref var spriteRef = ref stash_cellSprites.Get(cell);

                if (req.State == ChangeCellViewToPointerRequest.PointerState.Disabled)
                {
                    spriteRef.PointerLayer.gameObject.SetActive(false);
                }
                else if (req.State == ChangeCellViewToPointerRequest.PointerState.Enabled)
                {
                    spriteRef.PointerLayer.gameObject.SetActive(true);
                }
            }
        }

        private void HoverCells(ChangeCellViewToHoverRequest req)
        {
            foreach(var cell in req.Cells){
                if (cell.IsExist() == false) { continue; }
                if (stash_cellTag.Has(cell) == false) { continue; }
                if (stash_cellSprites.Has(cell) == false) { continue; }
                
                ref var spriteRef = ref stash_cellSprites.Get(cell);

                if(req.State == ChangeCellViewToHoverRequest.HoverState.Disabled){
                    spriteRef.HoverLayer.gameObject.SetActive(false);
                }
                else if (req.State == ChangeCellViewToHoverRequest.HoverState.Enabled){
                    spriteRef.HoverLayer.gameObject.SetActive(true);
                }
            }
        }
        
        private void SelectCells(ChangeCellViewToSelectRequest req){
            foreach (var cell in req.Cells)
            {
                if (cell.IsExist() == false) { continue; }
                if (stash_cellTag.Has(cell) == false) { continue; }
                if (stash_cellSprites.Has(cell) == false) { continue; }

                ref var spriteRef = ref stash_cellSprites.Get(cell);

                if (req.State == ChangeCellViewToSelectRequest.SelectState.Disabled)
                {
                    spriteRef.SelectedLayer.gameObject.SetActive(false);
                }
                else if (req.State == ChangeCellViewToSelectRequest.SelectState.Enabled)
                {
                    spriteRef.SelectedLayer.gameObject.SetActive(true);
                }
            }
        }

        private void ChangeColor(CellColorChangeRequest req){
            foreach(var cell in req.Cells){
                if(stash_cellTag.Has(cell) == false){ continue; }
                if(stash_spriteRef.Has(cell) == false){ continue; }
                
                if(req.ColorHex == "default")
                {
                    ReturnDefaultColor(req);
                    return;
                }
                
                ref var spriteRef = ref stash_spriteRef.Get(cell);
                spriteRef.Sprite.SetColor(req.ColorHex.ToColor());
            }
        }

        private void ReturnDefaultColor(CellColorChangeRequest req)
        {
            foreach(var cell in req.Cells){

                if (stash_cellTag.Has(cell) == false) { continue; }
                if (stash_spriteRef.Has(cell) == false) { continue; }

                ref var sprite = ref stash_spriteRef.Get(cell).Sprite;
                var defaultColor = sprite.GetDefaultColor();
                sprite.SetColor(defaultColor);
            }
        }
    }
}


