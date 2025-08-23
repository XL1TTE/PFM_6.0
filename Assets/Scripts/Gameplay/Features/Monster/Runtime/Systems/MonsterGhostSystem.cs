
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Events;
using Domain.Monster.Components;
using Domain.Monster.Mono;
using Domain.Monster.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Monster.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterGhostSystem : ISystem
    {
        public World World { get; set; }
        
        private MonsterDammy GhostCached = null;
                
        private Event<DragEndedEvent> evt_dragEnded;
        private Event<DragStartedEvent> evt_dragStarted;
        
        private Stash<MonsterDammyRefComponent> stash_monsterDammyRef;
        private Stash<DragStateComponent> stash_dragState;
        private Stash<TagMonster> stash_monsterTag;

        public void OnAwake()
        {
            evt_dragEnded = World.GetEvent<DragEndedEvent>();
            evt_dragStarted = World.GetEvent<DragStartedEvent>();

            stash_monsterDammyRef = World.GetStash<MonsterDammyRefComponent>();
            stash_dragState = World.GetStash<DragStateComponent>();
            stash_monsterTag = World.GetStash<TagMonster>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_dragStarted.publishedChanges){
                if(stash_monsterTag.Has(evt.DraggedEntity)){
                    var ghost = SpawnGhost(evt.DraggedEntity);
                    ghost.ChangeColor("#9F9F9F"); // gray and transparent color
                }
            }
            foreach(var evt in evt_dragEnded.publishedChanges){
                if(stash_monsterTag.Has(evt.DraggedEntity)){
                    DespawnGhost();
                }
            }
        }

        public void Dispose()
        {

        }
        
        private void DespawnGhost(){
            GhostCached.DestroySelf();
        }
        
        private MonsterDammy SpawnGhost(Entity monster){
            var dammy = stash_monsterDammyRef.Get(monster).MonsterDammy;
            var spawnPoint = stash_dragState.Get(monster).StartWorldPos;      
            GhostCached = Object.Instantiate(dammy, spawnPoint, dammy.transform.rotation);
            return GhostCached;
        }
        
    }
}


