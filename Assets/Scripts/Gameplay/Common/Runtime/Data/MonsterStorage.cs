
using Core.Components;
using Core.Utilities.Extantions;
using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.Monster;
using Gameplay.Features.Monster.Components;
using Persistence.Components;
using Persistence.DB;
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

                MonsterDammy Prefab = PrefabPath.LoadResource<MonsterDammy>();
                
                if(Prefab == null){
                    throw new System.Exception("Monster dammy prefab was not found!");
                }
                // Act
                MonsterDammy monster = UnityEngine.Object.Instantiate(Prefab);
                
                var legSpritePath = DataBase.Filter.With<LegSpritePath>().Build().First();
                var bodySpritePath = DataBase.Filter.With<BodySpritePath>().Build().First();

                monster.AttachFarLeg(DataBase
                    .GetRecord<LegSpritePath>(legSpritePath).FarSprite.LoadResource<Sprite>());
                monster.AttachBody(DataBase
                    .GetRecord<BodySpritePath>(bodySpritePath).path.LoadResource<Sprite>());

                TagMonster c_Monster = MonstersStash.Add(entity);
                ref TransformRefComponent c_Transform = ref TransformRefStash.Add(entity);
                ref DraggableComponent c_Draggable = ref DraggableStash.Add(entity);
                

                c_Transform.TransformRef = monster.transform;            

                c_Draggable.PickRadius = 0.5f;
                
                
                return entity;
            } 
        }    
    }

}
