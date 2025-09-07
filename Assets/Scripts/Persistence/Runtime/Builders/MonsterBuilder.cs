
using System.Collections.Generic;
using Domain.Abilities.Components;
using Domain.Components;
using Domain.Extentions;
using Domain.Monster.Components;
using Domain.Monster.Mono;
using Domain.Monster.Tags;
using Domain.Stats.Components;
using Domain.TurnSystem.Components;
using Persistence.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using UnityEngine;

namespace Persistence.Buiders
{

    public class MonsterBuilder: EntityBuilder
    {
        public MonsterBuilder(World ecsWorld){
            _ecsWorld = ecsWorld;

            stash_tagMonster = _ecsWorld.GetStash<TagMonster>();
            stash_moveAbility = _ecsWorld.GetStash<MovementAbility>();
            stash_attackAbility = _ecsWorld.GetStash<AttackAbility>();
            stash_transformRef = _ecsWorld.GetStash<TransformRefComponent>();
            stash_hitBox = _ecsWorld.GetStash<HitBoxComponent>();
            stash_monsterDammyRef = _ecsWorld.GetStash<MonsterDammyRefComponent>();
            stash_turnQueueAvatar = _ecsWorld.GetStash<TurnQueueAvatar>();
            stash_speed = _ecsWorld.GetStash<Speed>();

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
        Stash<AttackAbility> stash_attackAbility;
        Stash<TransformRefComponent> stash_transformRef;
        Stash<HitBoxComponent> stash_hitBox;
        Stash<MonsterDammyRefComponent> stash_monsterDammyRef;
        Stash<TurnQueueAvatar> stash_turnQueueAvatar;
        Stash<Speed> stash_speed;
        
        
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
            Entity monster_entity = _ecsWorld.CreateEntity();
            
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

            if(DataBase.TryGetRecord<PartsOffsets>(rec_body, out var bodyOffsets) == false){
                throw new System.Exception($"Body part Record {rec_body.Id} have not offsets!");
            }

            #region Sprite Attachment
            if (DataBase.TryGetRecord<BodySpritePath>(rec_body, out var spr_torso))
            {
                monsterDammy.AttachBody(spr_torso.path.LoadResource<Sprite>(), bodyOffsets.BodyOffset);
            }
            if (DataBase.TryGetRecord<HeadSpritePath>(rec_head, out var spr_head)){
                monsterDammy.AttachHead(spr_head.path.LoadResource<Sprite>(), bodyOffsets.HeadOffset);
            }
            if(DataBase.TryGetRecord<ArmSpritePath>(rec_farArm, out var spr_farArm)){
                monsterDammy.AttachFarArm(spr_farArm.FarSprite.LoadResource<Sprite>(), bodyOffsets.FarArmOffset);
            }
            if(DataBase.TryGetRecord<ArmSpritePath>(rec_nearArm, out var spr_nearArm)){
                monsterDammy.AttachNearArm(spr_nearArm.NearSprite.LoadResource<Sprite>(), bodyOffsets.NearArmOffset);
            }
            if(DataBase.TryGetRecord<LegSpritePath>(rec_farLeg, out var spr_farLeg)){
                monsterDammy.AttachFarLeg(spr_farLeg.FarSprite.LoadResource<Sprite>(), bodyOffsets.FarLegOffset);
            }
            if(DataBase.TryGetRecord<LegSpritePath>(rec_nearLeg, out var spr_nearLeg)){
                monsterDammy.AttachNearLeg(spr_nearLeg.NearSprite.LoadResource<Sprite>(), bodyOffsets.NearLegOffset);
            }

            #endregion

            #region Movement
            ref var moveAbility = ref stash_moveAbility.Add(monster_entity);
            var movementTemp = new HashSet<Vector2Int>();

            if (DataBase.TryGetRecord<MovementData>(rec_nearLeg, out var moveData_nearLeg))
            {
                foreach (var move in moveData_nearLeg.Movements)
                {
                    movementTemp.Add(move);
                }
            }
            if (DataBase.TryGetRecord<MovementData>(rec_farLeg, out var moveData_farLeg))
            {
                foreach (var move in moveData_farLeg.Movements)
                {
                    movementTemp.Add(move);
                }
            }

            moveAbility.Movements = new(movementTemp);

            stash_tagMonster.Add(monster_entity);

            #endregion

            #region  AttackAbility
            
            var attacks_temp = new HashSet<Vector2Int>();
            
            if(DataBase.TryGetRecord<AttackData>(rec_farArm, out var fArm_Attack)){
                foreach (var attack in fArm_Attack.Attacks){
                    attacks_temp.Add(attack);
                }
            }
            if(DataBase.TryGetRecord<AttackData>(rec_nearArm, out var nArm_Attack)){
                foreach (var attack in nArm_Attack.Attacks){
                    attacks_temp.Add(attack);
                }
            }

            stash_attackAbility.Add(monster_entity, new AttackAbility{
                Attacks = new(attacks_temp)
            });

            #endregion


            #region Speed

            float total_speed = 0.0f;
            
            if(DataBase.TryGetRecord<Speed>(rec_head, out var head_spead)){
                total_speed += head_spead.Value;
            }
            
            stash_speed.Add(monster_entity, new Speed{Value = total_speed});
            
            #endregion


            ref TransformRefComponent c_Transform = ref stash_transformRef.Add(monster_entity);
            c_Transform.Value = monsterDammy.transform;

            //stash_cursorDetector.Add(entity, new TagCursorDetector{DetectionRadius = 1.0f, DetectionPriority = 9999});

            stash_hitBox.Add(monster_entity, new HitBoxComponent{
                Offset = new Vector2(0, 20),
                Size = new Vector2(40, 60)
            });

            stash_monsterDammyRef.Add(monster_entity, new MonsterDammyRefComponent{MonsterDammy = monsterDammy });

            stash_turnQueueAvatar.Add(monster_entity, new TurnQueueAvatar{Value = monsterDammy.MonsterAvatar});

            return monster_entity;
            
            #endregion
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
