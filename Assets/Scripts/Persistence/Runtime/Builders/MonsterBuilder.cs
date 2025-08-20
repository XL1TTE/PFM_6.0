
using System.Collections.Generic;
using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Features.Monster;
using Gameplay.Features.Monster.Components;
using Persistence.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using UnityEngine;

namespace Persistence.Buiders{

    public class MonsterBuilder: EntityBuilder
    {
        public MonsterBuilder(World ecsWorld){
            _ecsWorld = ecsWorld;

            stash_tagMonster = _ecsWorld.GetStash<TagMonster>();
            stash_moveAbility = _ecsWorld.GetStash<MovementAbility>();
            stash_transformRef = _ecsWorld.GetStash<TransformRefComponent>();
            stash_cursorDetector = _ecsWorld.GetStash<TagCursorDetector>();
            stash_monsterDammyRef = _ecsWorld.GetStash<MonsterDammyRefComponent>();

            pfb_monsterDammy = _monsterDammyPath.LoadResource<MonsterDammy>();

            if (pfb_monsterDammy == null)
            {
                throw new System.Exception("Monster builder was not able to load monster dammy prefab resource.");
            }
            
            // _bodyRecords = DataBase.Filter.With<ID>().With<TagMonsterBody>().Build();
            // _headRecords = DataBase.Filter.With<ID>().With<TagMonsterHead>().Build();
            // _armsRecords = DataBase.Filter.With<ID>().With<TagMonsterArm>().Build();
            // _legsRecords = DataBase.Filter.With<ID>().With<TagMonsterLeg>().Build();
        }
        
        private const string _monsterDammyPath = "Monsters/Prefabs/MonsterDammy";
        private readonly MonsterDammy pfb_monsterDammy;
        
        private World _ecsWorld;
        
        Stash<TagMonster> stash_tagMonster;
        Stash<MovementAbility> stash_moveAbility;
        Stash<TransformRefComponent> stash_transformRef;
        Stash<TagCursorDetector> stash_cursorDetector;
        Stash<MonsterDammyRefComponent> stash_monsterDammyRef;
        
        
        
        
        #region Part_Ids
        private string id_head;
        private string id_body;
        private string id_nearArm;
        private string id_farArm;
        private string id_nearLeg;
        private string id_farLeg;
        #endregion

        public override Entity Build()
        {
            Entity entity = _ecsWorld.CreateEntity();
            
            MonsterDammy monsterDammy = UnityEngine.Object.Instantiate(pfb_monsterDammy);
            
            #region Part Attachment
            
            if(!DataBase.TryFindRecordByID(id_head, out var rec_head)){
                throw new System.Exception($"Record with id: {id_head} was not found.");
            }
            if(!DataBase.TryFindRecordByID(id_body, out var rec_body)){
                throw new System.Exception($"Record with id: {id_body} was not found.");
            }
            if(!DataBase.TryFindRecordByID(id_farArm, out var rec_farArm)){
                throw new System.Exception($"Record with id: {id_farArm} was not found.");
            }
            if(!DataBase.TryFindRecordByID(id_nearArm, out var rec_nearArm)){
                throw new System.Exception($"Record with id: {id_nearArm} was not found.");
            }
            if(!DataBase.TryFindRecordByID(id_farLeg, out var rec_farLeg)){
                throw new System.Exception($"Record with id: {id_farLeg} was not found.");
            }
            if(!DataBase.TryFindRecordByID(id_nearLeg, out var rec_nearLeg)){
                throw new System.Exception($"Record with id: {id_nearLeg} was not found.");
            }
            
            #region Sprite Attachment
            
            if(DataBase.TryGetRecord<HeadSpritePath>(rec_head, out var spr_head)){
                monsterDammy.AttachHead(spr_head.path.LoadResource<Sprite>());
            }
            if(DataBase.TryGetRecord<BodySpritePath>(rec_body, out var spr_torso)){
                monsterDammy.AttachBody(spr_torso.path.LoadResource<Sprite>());
            }
            if(DataBase.TryGetRecord<ArmSpritePath>(rec_farArm, out var spr_farArm)){
                monsterDammy.AttachFarArm(spr_farArm.FarSprite.LoadResource<Sprite>());
            }
            if(DataBase.TryGetRecord<ArmSpritePath>(rec_nearArm, out var spr_nearArm)){
                monsterDammy.AttachNearArm(spr_nearArm.NearSprite.LoadResource<Sprite>());
            }
            if(DataBase.TryGetRecord<LegSpritePath>(rec_farLeg, out var spr_farLeg)){
                monsterDammy.AttachFarLeg(spr_farLeg.FarSprite.LoadResource<Sprite>());
            }
            if(DataBase.TryGetRecord<LegSpritePath>(rec_nearLeg, out var spr_nearLeg)){
                monsterDammy.AttachNearLeg(spr_nearLeg.NearSprite.LoadResource<Sprite>());
            }

            #endregion

            ref var moveAbility = ref stash_moveAbility.Add(entity);     
            var movementTemp = new HashSet<Vector2Int>();

            if(DataBase.TryGetRecord<MovementData>(rec_nearLeg, out var moveData_nearLeg)){
                foreach(var move in moveData_nearLeg.Movements){
                    movementTemp.Add(move);
                }
            }
            if(DataBase.TryGetRecord<MovementData>(rec_farLeg, out var moveData_farLeg)){
                foreach (var move in moveData_farLeg.Movements){
                    movementTemp.Add(move);
                }
            }
            
            moveAbility.Movements = new(movementTemp);

            #endregion

            stash_tagMonster.Add(entity);
            
            ref TransformRefComponent c_Transform = ref stash_transformRef.Add(entity);
            c_Transform.TransformRef = monsterDammy.transform;

            //stash_cursorDetector.Add(entity, new TagCursorDetector{DetectionRadius = 1.0f, DetectionPriority = 9999});

            stash_monsterDammyRef.Add(entity, new MonsterDammyRefComponent{MonsterDammy = monsterDammy });

            return entity;
        }
        
        public MonsterBuilder AttachHead(string head_id) {
            id_head = head_id;
            return this;
        } 
        public MonsterBuilder AttachBody(string body_id) {
            id_body = body_id;
            return this;
        } 
        public MonsterBuilder AttachNearArm(string nearArm_id) {
            id_nearArm = nearArm_id;
            return this;
        } 
        public MonsterBuilder AttachFarArm(string farArm_id) {
            id_farArm = farArm_id;
            return this;
        } 
        public MonsterBuilder AttachNearLeg(string nearLeg_id) {
            id_nearLeg = nearLeg_id;
            return this;
        } 
        public MonsterBuilder AttachFarLeg(string farLeg_id) {
            id_farLeg = farLeg_id;
            return this;
        } 
    }
}
