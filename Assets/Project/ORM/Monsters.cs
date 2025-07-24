

using System.Collections;
using ECS.Components;
using ECS.Components.Monsters;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityUtilities;

namespace Project
{
    public static partial class Domain
    {
        public static class Monsters
        {
            public static class Entity{
                public static Scellecs.Morpeh.Entity CreateNew(string PrefabPath){
                    //Arrange
                    Scellecs.Morpeh.Entity entity = _ecsWorld.CreateEntity();

                    Stash<SpriteComponent> SpriteStash = _ecsWorld.GetStash<SpriteComponent>();
                    Stash<TransformComponent> TransformStash = _ecsWorld.GetStash<TransformComponent>();
                    
                    GameObject Prefab = PrefabPath.LoadResource<GameObject>();
                    
                    // Act
                    GameObject monster = UnityEngine.Object.Instantiate(Prefab);
                    
                    ref SpriteComponent c_Sprite = ref SpriteStash.Add(entity);
                    ref TransformComponent c_Transform = ref TransformStash.Add(entity);
                    
                    Prefab.TryFindComponent(out c_Sprite.Sprite);
                    c_Transform.Transform = monster.transform;

                    return entity;
                } 
            }    
        }
    }

}
