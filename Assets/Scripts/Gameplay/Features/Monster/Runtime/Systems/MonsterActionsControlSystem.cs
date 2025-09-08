
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
    public sealed class  MonsterActionsControlSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter f_monsters;
        
        private Stash<Attacking> stash_attacking;
        private Stash<Moving> stash_moving;
        private Stash<PerfomingActionTag> stash_perfomingAction;

        public void OnAwake()
        {
            f_monsters = World.Filter
                .With<TagMonster>().Build();

            stash_attacking = World.GetStash<Attacking>();
            stash_moving = World.GetStash<Moving>();
            stash_perfomingAction = World.GetStash<PerfomingActionTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var monster in f_monsters){
                bool actionPerformed = false;
                
                if (stash_attacking.Has(monster)){
                    actionPerformed = true;
                }
                if(stash_moving.Has(monster)){
                    actionPerformed = true;
                }
                
                
                if(actionPerformed){
                    MarkAsPerformingAction(monster);
                }
                else{
                    UnMarkAsPerformingAction(monster);
                }
            }
        }

        public void Dispose()
        {

        }
        
        private void MarkAsPerformingAction(Entity entity){
            stash_perfomingAction.Set(entity, new PerfomingActionTag{});
        }
        private void UnMarkAsPerformingAction(Entity entity){
            if(stash_perfomingAction.Has(entity)){
                stash_perfomingAction.Remove(entity);
            }
        }
        
        
    }
}


