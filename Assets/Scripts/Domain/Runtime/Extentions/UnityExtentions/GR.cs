using Domain.Abilities.Mono;
using Domain.FloatingDamage;
using Domain.UI.Widgets;
using UI.Elements;
using UnityEngine;

namespace Domain.Extentions
{
    public static partial class GR
    {

        public readonly static HealthBarView p_MonsterHealthBar = "UI/MonsterHealthBar".LoadResource<HealthBarView>();
        public readonly static HealthBarView p_EnemyHealthBar = "UI/EnemyHealthBar".LoadResource<HealthBarView>();


        public readonly static FloatingDamageView p_FloatingDamage = "UI/FloatingDamage".LoadResource<FloatingDamageView>();


        public readonly static AbilityButtonView p_AbilityButton = "Abilities/AbilityButton".LoadResource<AbilityButtonView>();

        public readonly static HorizontalLayoutElement p_ToolTipLine = "UI/ToolTipLine".LoadResource<HorizontalLayoutElement>();

        public readonly static Sprite SPR_UI_PHYSICAL_DMG = "Art/Effects/Spr_UI_Effect_Debuff".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_BLOOD = "Art/Effects/Spr_UI_Effect_Blood".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_FIRE = "Art/Effects/Spr_UI_Effect_Fire".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_POISON = "Art/Effects/Spr_UI_Effect_Poison".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_STUNNED = "Art/Effects/Spr_UI_Effect_Stun".LoadResource<Sprite>();


        public readonly static Sprite SPR_UI_FIRE_RES_NONE
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_fire_res_none");
        public readonly static Sprite SPR_UI_FIRE_RES_RESISTANT
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_fire_res_resistant");
        public readonly static Sprite SPR_UI_FIRE_RES_IMMUNED
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_fire_res_immuned");

        public readonly static Sprite SPR_UI_BLEED_RES_NONE
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_bleed_res_none");
        public readonly static Sprite SPR_UI_BLEED_RES_RESISTANT
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_bleed_res_resistant");
        public readonly static Sprite SPR_UI_BLEED_RES_IMMUNED
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_bleed_res_immuned");

        public readonly static Sprite SPR_UI_POISON_RES_NONE
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_poison_res_none");
        public readonly static Sprite SPR_UI_POISON_RES_RESISTANT
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_poison_res_resistant");
        public readonly static Sprite SPR_UI_POISON_RES_IMMUNED
            = "Art/Spr_Sheet_Resistances".LoadFromSheet("spr_poison_res_immuned");


        public readonly static Sprite SPR_UI_AVATAR_CAT
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_cat");
        public readonly static Sprite SPR_UI_AVATAR_DOG
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_dog");
        public readonly static Sprite SPR_UI_AVATAR_GOOSE
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_goose");
        public readonly static Sprite SPR_UI_AVATAR_HORSE
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_horse");
        public readonly static Sprite SPR_UI_AVATAR_PIG
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_pig");
        public readonly static Sprite SPR_UI_AVATAR_ROOSTER
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_rooster");
        public readonly static Sprite SPR_UI_AVATAR_GOAT
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_goat");
        public readonly static Sprite SPR_UI_AVATAR_SHEEP
            = "Monsters/Avatars/Spr_Sheet_MonsterAvatars".LoadFromSheet("spr_sheep");



        #region BodyParts
        public readonly static Sprite SPR_BP_FLEG_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_fLeg_Rat");
        public readonly static Sprite SPR_BP_NLEG_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_nLeg_Rat");
        public readonly static Sprite SPR_BP_FARM_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_fArm_Rat");
        public readonly static Sprite SPR_BP_NARM_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_nArm_Rat");

        public readonly static Sprite SPR_BP_FLEG_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_TORSO_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_HEAD_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_Head");

        public readonly static Sprite SPR_BP_NARM_COCKROACH
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cockroach".LoadFromSheet("Spr_Bp_nArm");
        public readonly static Sprite SPR_BP_FARM_COCKROACH
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cockroach".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_COW
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cow".LoadFromSheet("Spr_Bp_nArm");
        public readonly static Sprite SPR_BP_FARM_COW
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cow".LoadFromSheet("Spr_Bp_fArm");
        #endregion


        public readonly static Sprite SPR_UI_ABT_ATTACK
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_attack");
        public readonly static Sprite SPR_UI_ABT_EFFECT
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_effect");
        public readonly static Sprite SPR_UI_ABT_ROTATE
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_rotate");
        public readonly static Sprite SPR_UI_ABT_MOVE
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_move");
        public readonly static Sprite SPR_UI_ABT_HEAL
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_heal");
        public readonly static Sprite SPR_UI_ABT_COCKROACH_SPIT
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cockroach-spit");
        public readonly static Sprite SPR_UI_ABT_COCKROACH_RADIOACTIVE
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_radioactive");
        public readonly static Sprite SPR_UI_ABT_SHEEP_ARM
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_sheep-arm");
        public readonly static Sprite SPR_UI_ABT_SHEEP_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_sheep-head");

        public readonly static Sprite SPR_UI_ABT_RAT_ARM
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_ratArm");
        public readonly static Sprite SPR_UI_ABT_RAT_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_ratHead");
        public readonly static Sprite SPR_UI_ABT_PIG_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_pigHead");
        public readonly static Sprite SPR_UI_ABT_PIG_ARM
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_pig-arm");
    }
}
