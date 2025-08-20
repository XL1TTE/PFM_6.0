using System;
using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Providers;
using Gameplay.Features.BattleField.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Features.BattleField.Systems{
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CellsViewSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<CellColorChangeRequest> req_ChangeColor;
        private Request<CellSpriteChangeRequest> req_ChangeSprite;
        
        private Stash<CellTag> stash_cellTag;
        private Stash<SpriteComponent> stash_spriteRef;
        private Stash<CellSpritesComponent> stash_cellSprites;

        public void OnAwake()
        {
            req_ChangeColor = World.GetRequest<CellColorChangeRequest>();
            req_ChangeSprite = World.GetRequest<CellSpriteChangeRequest>();

            stash_cellTag = World.GetStash<CellTag>();
            stash_spriteRef = World.GetStash<SpriteComponent>();
            stash_cellSprites = World.GetStash<CellSpritesComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_ChangeColor.Consume()){
                ChangeColor(req);
            }
            foreach(var req in req_ChangeSprite.Consume()){
                ChangeSprite(req);
            }
        }

        public void Dispose()
        {

        }

        private Sprite GetSpriteForCell(CellSpriteChangeRequest.SpriteType spriteType, Entity cell){
            Sprite sprite = null;
                    
            ref var cellSprites = ref stash_cellSprites.Get(cell);
            var spriteController = stash_spriteRef.Get(cell).Sprite;
            
            switch (spriteType)
            {
                case CellSpriteChangeRequest.SpriteType.Default:
                    cellSprites.SpriteState = CellSpritesComponent.SpriteStates.Default;
                    sprite = cellSprites.EmptySprite;
                    break;
                case CellSpriteChangeRequest.SpriteType.Previous:
                    cellSprites.SpriteState = cellSprites.PreviousSpriteState;
                    sprite = spriteController.GetPreviousSprite();
                    break;
                case CellSpriteChangeRequest.SpriteType.Hover:
                    cellSprites.SpriteState = CellSpritesComponent.SpriteStates.Hovered;
                    sprite = cellSprites.HoverSprite;
                    break;
                case CellSpriteChangeRequest.SpriteType.Highlighted:
                    cellSprites.SpriteState = CellSpritesComponent.SpriteStates.Highlighted;
                    sprite = cellSprites.HighlightedSprite;
                    break;
            }
            
            return sprite;
        }

        private void ChangeSprite(CellSpriteChangeRequest req)
        {
            foreach(var cell in req.Cells){
                if (stash_cellTag.Has(cell) == false) { continue; }
                if (stash_spriteRef.Has(cell) == false) { continue; }
                if (stash_cellSprites.Has(cell) == false) { continue; }

                var sprite = GetSpriteForCell(req.Sprite, cell);

                stash_spriteRef.Get(cell).Sprite.SetSprite(sprite);
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


