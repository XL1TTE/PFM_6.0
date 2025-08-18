using System;
using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Features.BattleField.Systems{
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CellsViewSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<CellColorChangeRequest> req_ChangeColor;
        
        private Stash<TagBattleFieldCell> stash_cellTag;
        private Stash<SpriteComponent> stash_spriteRef;

        public void OnAwake()
        {
            req_ChangeColor = World.GetRequest<CellColorChangeRequest>();

            stash_cellTag = World.GetStash<TagBattleFieldCell>();
            stash_spriteRef = World.GetStash<SpriteComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_ChangeColor.Consume()){
                ChangeColor(req);
            }
        }

        public void Dispose()
        {

        }
        
        private void ChangeColor(CellColorChangeRequest req){
            foreach(var cell in req.Cells){
                if(stash_cellTag.Has(cell) == false){continue;}
                if(stash_spriteRef.Has(cell) == false){continue;}
                
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
                ref var sprite = ref stash_spriteRef.Get(cell).Sprite;
                var defaultColor = sprite.GetDefaultColor();
                sprite.SetColor(defaultColor);
            }
        }
    }
}


