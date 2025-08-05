
using Core.Components;
using Core.Utilities.Extantions;
using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.Monster.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.Common
{
    public static class MonsterStorage
    {
        static MonsterStorage(){
            _ecsWorld = World.Default;
        }
        
        private static World _ecsWorld;
        
        public static class Entity{
            public static Scellecs.Morpeh.Entity CreateNew(string PrefabPath){
                //Arrange
                Scellecs.Morpeh.Entity entity = _ecsWorld.CreateEntity();

                Stash<TagMonster> MonstersStash = _ecsWorld.GetStash<TagMonster>();
                Stash<SpriteComponent> SpriteStash = _ecsWorld.GetStash<SpriteComponent>();
                Stash<TransformRefComponent> TransformRefStash = _ecsWorld.GetStash<TransformRefComponent>();
                Stash<DraggableComponent> DraggableStash = _ecsWorld.GetStash<DraggableComponent>();
                
                GameObject Prefab = PrefabPath.LoadResource<GameObject>();
                
                // Act
                GameObject monster = UnityEngine.Object.Instantiate(Prefab);
                
                ref SpriteComponent c_Sprite = ref SpriteStash.Add(entity);
                TagMonster c_Monster = MonstersStash.Add(entity);
                ref TransformRefComponent c_Transform = ref TransformRefStash.Add(entity);
                ref DraggableComponent c_Draggable = ref DraggableStash.Add(entity);
                

                monster.TryFindComponent(out c_Sprite.Sprite);
                c_Transform.TransformRef = monster.transform;            

                c_Draggable.PickRadius = 0.5f;
                
                
                return entity;
            } 
        }    
    }

}
