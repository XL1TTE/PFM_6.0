
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Domain.Abilities.Components;
using Domain.Components;
using Domain.Extentions;
using Domain.GameEffects;
using Domain.HealthBars.Components;
using Domain.Monster.Components;
using Domain.Monster.Mono;
using Domain.Monster.Tags;
using Domain.Stats.Components;
using Domain.TurnSystem.Components;
using Interactions;
using Persistence.Components;
using Persistence.DB;
using Persistence.Utilities;
using Scellecs.Morpeh;
using UnityEngine;

namespace Persistence.Buiders
{

    public class MonsterBuilder : EntityBuilder
    {
        public MonsterBuilder(World ecsWorld)
        {
            _ecsWorld = ecsWorld;

            stash_tagMonster = _ecsWorld.GetStash<TagMonster>();
            stash_lookDir = _ecsWorld.GetStash<LookDirection>();
            stash_moveAbility = _ecsWorld.GetStash<MovementAbility>();
            //stash_attackAbility = _ecsWorld.GetStash<AttackAbility>();
            stash_transformRef = _ecsWorld.GetStash<TransformRefComponent>();
            stash_hitBox = _ecsWorld.GetStash<HitBoxComponent>();
            stash_monsterDammyRef = _ecsWorld.GetStash<MonsterDammyRefComponent>();
            stash_turnQueueAvatar = _ecsWorld.GetStash<TurnQueueAvatar>();
            stash_initEffectsPool = _ecsWorld.GetStash<InitialEffectsPoolComponent>();
            stash_iHaveHealthBar = _ecsWorld.GetStash<IHaveHealthBar>();

            stash_abilities = _ecsWorld.GetStash<AbilitiesComponent>();

            stash_Speed = _ecsWorld.GetStash<Speed>();
            stash_Health = _ecsWorld.GetStash<Health>();
            stash_MaxHealt = _ecsWorld.GetStash<MaxHealth>();

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
        private Stash<LookDirection> stash_lookDir;
        Stash<MovementAbility> stash_moveAbility;
        //Stash<AttackAbility> stash_attackAbility;
        Stash<TransformRefComponent> stash_transformRef;
        Stash<HitBoxComponent> stash_hitBox;
        Stash<MonsterDammyRefComponent> stash_monsterDammyRef;
        Stash<TurnQueueAvatar> stash_turnQueueAvatar;

        Stash<InitialEffectsPoolComponent> stash_initEffectsPool;
        private readonly Stash<IHaveHealthBar> stash_iHaveHealthBar;
        private readonly Stash<AbilitiesComponent> stash_abilities;
        private readonly Stash<Speed> stash_Speed;


        #region Part_Ids
        private string id_head;
        private string id_body;
        private string id_nearArm;
        private string id_farArm;
        private string id_nearLeg;
        private string id_farLeg;
        private Stash<Health> stash_Health;
        private readonly Stash<MaxHealth> stash_MaxHealt;
        #endregion

        public override Entity Build()
        {
            Entity monster_entity = _ecsWorld.CreateEntity();

            MonsterDammy monsterDammy = UnityEngine.Object.Instantiate(pfb_monsterDammy);

            #region Part Attachment

            if (!DataBase.TryFindRecordByID(id_head, out var rec_head))
            {
                throw new System.Exception($"Record with id: {id_head} was not found.");
            }
            if (!DataBase.TryFindRecordByID(id_body, out var rec_body))
            {
                throw new System.Exception($"Record with id: {id_body} was not found.");
            }
            if (!DataBase.TryFindRecordByID(id_farArm, out var rec_farArm))
            {
                throw new System.Exception($"Record with id: {id_farArm} was not found.");
            }
            if (!DataBase.TryFindRecordByID(id_nearArm, out var rec_nearArm))
            {
                throw new System.Exception($"Record with id: {id_nearArm} was not found.");
            }
            if (!DataBase.TryFindRecordByID(id_farLeg, out var rec_farLeg))
            {
                throw new System.Exception($"Record with id: {id_farLeg} was not found.");
            }
            if (!DataBase.TryFindRecordByID(id_nearLeg, out var rec_nearLeg))
            {
                throw new System.Exception($"Record with id: {id_nearLeg} was not found.");
            }

            if (DataBase.TryGetRecord<PartsOffsets>(rec_body, out var bodyOffsets) == false)
            {
                throw new System.Exception($"Body part Record {rec_body.Id} have not offsets!");
            }

            #region Sprite Attachment
            if (DataBase.TryGetRecord<BodySpritePath>(rec_body, out var spr_torso))
            {
                monsterDammy.AttachBody(spr_torso.path.LoadResource<Sprite>(), bodyOffsets.BodyOffset);
            }
            if (DataBase.TryGetRecord<HeadSpritePath>(rec_head, out var spr_head))
            {
                monsterDammy.AttachHead(spr_head.path.LoadResource<Sprite>(), bodyOffsets.HeadOffset);
            }
            if (DataBase.TryGetRecord<ArmSpritePath>(rec_farArm, out var spr_farArm))
            {
                monsterDammy.AttachFarArm(spr_farArm.FarSprite.LoadResource<Sprite>(), bodyOffsets.FarArmOffset);
            }
            if (DataBase.TryGetRecord<ArmSpritePath>(rec_nearArm, out var spr_nearArm))
            {
                monsterDammy.AttachNearArm(spr_nearArm.NearSprite.LoadResource<Sprite>(), bodyOffsets.NearArmOffset);
            }
            if (DataBase.TryGetRecord<LegSpritePath>(rec_farLeg, out var spr_farLeg))
            {
                monsterDammy.AttachFarLeg(spr_farLeg.FarSprite.LoadResource<Sprite>(), bodyOffsets.FarLegOffset);
            }
            if (DataBase.TryGetRecord<LegSpritePath>(rec_nearLeg, out var spr_nearLeg))
            {
                monsterDammy.AttachNearLeg(spr_nearLeg.NearSprite.LoadResource<Sprite>(), bodyOffsets.NearLegOffset);
            }

            #endregion


            stash_tagMonster.Add(monster_entity);

            stash_lookDir.Set(monster_entity, new LookDirection { m_Value = Directions.RIGHT });

            AbilitiesComponent t_abilities = new AbilitiesComponent();
            AbilityData headAbtData = null;
            AbilityData rArmAbtData = null;
            AbilityData lArmAbtData = null;
            AbilityData lLegAbtData = null;
            AbilityData rLegAbtData = null;
            AbilityData LegAbtData = null;
            if (DataBase.TryGetRecord<AbilityProvider>(rec_head, out var head_Ability))
            {
                headAbtData = DbUtility.GetAbilityDataFromAbilityRecord(head_Ability.m_AbilityTemplateID);
            }
            if (DataBase.TryGetRecord<AbilityProvider>(rec_farArm, out var fArm_Ability))
            {
                lArmAbtData = DbUtility.GetAbilityDataFromAbilityRecord(fArm_Ability.m_AbilityTemplateID);
            }
            if (DataBase.TryGetRecord<AbilityProvider>(rec_nearArm, out var nArm_Ability))
            {
                rArmAbtData = DbUtility.GetAbilityDataFromAbilityRecord(nArm_Ability.m_AbilityTemplateID);
            }

            if (rArmAbtData != null && lArmAbtData != null && nArm_Ability.m_AbilityTemplateID == fArm_Ability.m_AbilityTemplateID)
            {
                DbUtility.DoubleAbilityStats(ref rArmAbtData.m_Value);
                DbUtility.DoubleAbilityStats(ref lArmAbtData.m_Value);
            }

            if (DataBase.TryGetRecord<AbilityProvider>(rec_nearLeg, out var nleg_Ability))
            {
                lLegAbtData = DbUtility.GetAbilityDataFromAbilityRecord(nleg_Ability.m_AbilityTemplateID);
            }
            if (DataBase.TryGetRecord<AbilityProvider>(rec_farLeg, out var fleg_Ability))
            {
                rLegAbtData = DbUtility.GetAbilityDataFromAbilityRecord(fleg_Ability.m_AbilityTemplateID);
            }

            if (rLegAbtData != null && rLegAbtData != null && nleg_Ability.m_AbilityTemplateID == fleg_Ability.m_AbilityTemplateID)
            {
                var movements = DbUtility.CombineShifts(lLegAbtData.m_Shifts, rLegAbtData.m_Shifts);
                LegAbtData = lLegAbtData;
                LegAbtData.m_Shifts = movements;
            }

            t_abilities.m_HeadAbility = headAbtData;
            t_abilities.m_LeftHandAbility = lArmAbtData;
            t_abilities.m_RightHandAbility = rArmAbtData;
            t_abilities.m_LegsAbility = LegAbtData;

            t_abilities.m_TurnAroundAbility =
                DbUtility.GetAbilityDataFromAbilityRecord(L.TURN_AROUND_ABILITY_ID);

            stash_abilities.Set(monster_entity, t_abilities);

            #endregion


            #region Effects

            var all_effects = new List<string>();

            if (DataBase.TryGetRecord<EffectsProvider>(rec_head, out var head_effects))
            {
                all_effects.AddRange(head_effects.m_Effects);
            }

            stash_initEffectsPool.Set(monster_entity, new InitialEffectsPoolComponent
            {
                m_StatusEffects = new(),
                m_PermanentEffects = all_effects.Select((e_id) => new PermanentEffect { m_EffectId = e_id }).ToList()
            });

            stash_iHaveHealthBar.Set(monster_entity, new IHaveHealthBar
            {
                HealthBarPrefab = GR.p_MonsterHealthBar
            });

            #endregion


            ref TransformRefComponent c_Transform = ref stash_transformRef.Add(monster_entity);
            c_Transform.Value = monsterDammy.transform;


            stash_hitBox.Add(monster_entity, new HitBoxComponent
            {
                Offset = new Vector2(0, 20),
                Size = new Vector2(40, 60)
            });

            stash_monsterDammyRef.Add(monster_entity, new MonsterDammyRefComponent { MonsterDammy = monsterDammy });

            stash_turnQueueAvatar.Add(monster_entity, new TurnQueueAvatar { Value = monsterDammy.MonsterAvatar });

            return monster_entity;

        }

        public MonsterBuilder AttachHead(string head_id)
        {
            id_head = head_id;
            return this;
        }
        public MonsterBuilder AttachBody(string body_id)
        {
            id_body = body_id;
            return this;
        }
        public MonsterBuilder AttachNearArm(string nearArm_id)
        {
            id_nearArm = nearArm_id;
            return this;
        }
        public MonsterBuilder AttachFarArm(string farArm_id)
        {
            id_farArm = farArm_id;
            return this;
        }
        public MonsterBuilder AttachNearLeg(string nearLeg_id)
        {
            id_nearLeg = nearLeg_id;
            return this;
        }
        public MonsterBuilder AttachFarLeg(string farLeg_id)
        {
            id_farLeg = farLeg_id;
            return this;
        }
    }
}
