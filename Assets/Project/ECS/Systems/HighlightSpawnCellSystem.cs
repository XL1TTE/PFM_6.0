using ECS.Components;
using ECS.Components.Monsters;
using ECS.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class HighlightSpawnCellSystem : ISystem 
{
    public World World { get; set;}
    
    private Filter _spawnCells;
    
    private Request<EnableMonsterSpawnCellsHighlightRequest> _enableHighlightRequest;
    private Request<DisableMonsterSpawnCellsHighlightRequest> _disableHighlightRequest;
    
    private Stash<SpriteComponent> _spriteStash;
    
    
    private Stash<DropTargetComponent> stash_dropTarget;
    
    
    private float _colorIntensityMultiplier = 1.2f;


    public void OnAwake() 
    {
        _enableHighlightRequest = World.GetRequest<EnableMonsterSpawnCellsHighlightRequest>();
        _disableHighlightRequest = World.GetRequest<DisableMonsterSpawnCellsHighlightRequest>();

        _spawnCells = World.Filter
            .With<TagMonsterSpawnCell>()
            .With<SpriteComponent>()
            .Build();

        _spriteStash = World.GetStash<SpriteComponent>();
        stash_dropTarget = World.GetStash<DropTargetComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        if(_spawnCells.IsEmpty()){return;}
        
        // enable highlight
        foreach(var req in _enableHighlightRequest.Consume()){
            foreach (var e in _spawnCells)
            {
                ref SpriteComponent c_sprite = ref _spriteStash.Get(e);
                var sprite = c_sprite.Sprite;
                var color = sprite.GetColor();

                Color newColor = new Color(color.r * _colorIntensityMultiplier,
                            color.g * _colorIntensityMultiplier,
                            color.b * _colorIntensityMultiplier,
                            color.a);
                
                sprite.SetColor(newColor);     
                
            }

            MakeCellsDropTargets();
        }

        // disable highlight
        foreach (var req in _disableHighlightRequest.Consume())
        {
            foreach (var e in _spawnCells)
            {
                ref SpriteComponent c_sprite = ref _spriteStash.Get(e);
                var sprite = c_sprite.Sprite;
                var color = sprite.GetColor();

                Color newColor = new Color(color.r / _colorIntensityMultiplier,
                            color.g / _colorIntensityMultiplier,
                            color.b / _colorIntensityMultiplier,
                            color.a);

                sprite.SetColor(newColor);

            }
        }
    }

    public void Dispose()
    {

    }
    
    
    private void MakeCellsDropTargets(){
        foreach (var e in _spawnCells)
        {
            if(!stash_dropTarget.Has(e)){
                ref var dropTarget = ref stash_dropTarget.Add(e);
                dropTarget.DropRadius = 1.0f;
            }
        }
    }
}
