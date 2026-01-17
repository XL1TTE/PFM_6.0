using Domain.Abilities.Mono;
using Domain.FloatingDamage;
using Domain.UI.Widgets;
using UI.Elements;
using UnityEngine;

namespace Domain.Extentions
{
    public static partial class GR
    {
        #region base

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
        
        #endregion


        #region BodyParts Icons

        #region Bear
        public readonly static Sprite SPR_UI_BP_ARM_BEAR
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_bear");
        public readonly static Sprite SPR_UI_BP_LEG_BEAR
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_bear");
        #endregion

        #region Bee
        public readonly static Sprite SPR_UI_BP_ARM_BEE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_bee");
        public readonly static Sprite SPR_UI_BP_TORSO_BEE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_bee");
        #endregion

        #region Cat
        public readonly static Sprite SPR_UI_BP_HEAD_CAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_cat");
        public readonly static Sprite SPR_UI_BP_ARM_CAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_cat");
        public readonly static Sprite SPR_UI_BP_LEG_CAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_cat");
        #endregion

        #region Cockroach
        public readonly static Sprite SPR_UI_BP_ARM_COCKROACH
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_cockroach");
        #endregion

        #region Cow
        public readonly static Sprite SPR_UI_BP_ARM_COW
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_cow");
        #endregion

        #region Dog
        public readonly static Sprite SPR_UI_BP_HEAD_DOG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_dog");
        public readonly static Sprite SPR_UI_BP_TORSO_DOG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_dog");
        public readonly static Sprite SPR_UI_BP_LEG_DOG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_dog");
        #endregion

        #region Dove
        public readonly static Sprite SPR_UI_BP_ARM_DOVE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_dove");
        #endregion

        #region Goat
        public readonly static Sprite SPR_UI_BP_HEAD_GOAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_goat");
        public readonly static Sprite SPR_UI_BP_ARM_GOAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_goat");
        public readonly static Sprite SPR_UI_BP_LEG_GOAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_goat");
        #endregion

        #region Goose
        public readonly static Sprite SPR_UI_BP_HEAD_GOOSE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_goose");
        public readonly static Sprite SPR_UI_BP_LEG_GOOSE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_goose");
        #endregion

        #region Horse
        public readonly static Sprite SPR_UI_BP_HEAD_HORSE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_horse");
        public readonly static Sprite SPR_UI_BP_LEG_HORSE
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_horse");
        #endregion

        #region Ladybug
        public readonly static Sprite SPR_UI_BP_ARM_LADYBUG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_ladybug");
        public readonly static Sprite SPR_UI_BP_LEG_LADYBUG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_ladybug");
        #endregion

        #region Pig
        public readonly static Sprite SPR_UI_BP_HEAD_PIG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_pig");
        public readonly static Sprite SPR_UI_BP_TORSO_PIG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_pig");
        public readonly static Sprite SPR_UI_BP_LEG_PIG
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_pig");
        #endregion

        #region Raccoon
        public readonly static Sprite SPR_UI_BP_TORSO_RACCOON
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_raccoon");
        public readonly static Sprite SPR_UI_BP_ARM_RACCOON
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_raccoon");
        public readonly static Sprite SPR_UI_BP_LEG_RACCOON
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_raccoon");
        #endregion

        #region Rat
        public readonly static Sprite SPR_UI_BP_ARM_RAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_rat");
        public readonly static Sprite SPR_UI_BP_LEG_RAT
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_rat");
        #endregion

        #region Raven
        public readonly static Sprite SPR_UI_BP_TORSO_RAVEN
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_raven");
        public readonly static Sprite SPR_UI_BP_ARM_RAVEN
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_raven");
        public readonly static Sprite SPR_UI_BP_LEG_RAVEN
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_raven");
        #endregion

        #region Rooster
        public readonly static Sprite SPR_UI_BP_HEAD_ROOSTER
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_rooster");
        public readonly static Sprite SPR_UI_BP_TORSO_ROOSTER
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_rooster");
        public readonly static Sprite SPR_UI_BP_ARM_ROOSTER
           = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_rooster");
        public readonly static Sprite SPR_UI_BP_LEG_ROOSTER
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_rooster");
        #endregion

        #region Sheep
        public readonly static Sprite SPR_UI_BP_HEAD_SHEEP
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_head_sheep");
        public readonly static Sprite SPR_UI_BP_TORSO_SHEEP
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_torso_sheep");
        public readonly static Sprite SPR_UI_BP_ARM_SHEEP
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_arm_sheep");
        public readonly static Sprite SPR_UI_BP_LEG_SHEEP
            = "Art/BodyParts/Icons/Spr_Sheet_BodyParts".LoadFromSheet("spr_ui_bp_leg_sheep");
        #endregion

        #endregion


        #region BodyParts

        #region Bear
        public readonly static Sprite SPR_BP_FLEG_BEAR
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bear".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_BEAR
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bear".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_BEAR
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bear".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_BEAR
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bear".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Bee
        public readonly static Sprite SPR_BP_TORSO_BEE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bee".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_FARM_BEE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bee".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_BEE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Bee".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Cat
        public readonly static Sprite SPR_BP_HEAD_CAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cat".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_CAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cat".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_CAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cat".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_CAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cat".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_CAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cat".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Cockroach
        public readonly static Sprite SPR_BP_NARM_COCKROACH
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cockroach".LoadFromSheet("Spr_Bp_nArm");
        public readonly static Sprite SPR_BP_FARM_COCKROACH
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cockroach".LoadFromSheet("Spr_Bp_fArm");
        #endregion

        #region Cow
        public readonly static Sprite SPR_BP_NARM_COW
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cow".LoadFromSheet("Spr_Bp_nArm");
        public readonly static Sprite SPR_BP_FARM_COW
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Cow".LoadFromSheet("Spr_Bp_fArm");
        #endregion

        #region Dog
        public readonly static Sprite SPR_BP_TORSO_DOG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Dog".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_HEAD_DOG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Dog".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_DOG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Dog".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_DOG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Dog".LoadFromSheet("Spr_Bp_nLeg");
        #endregion

        #region Dove
        public readonly static Sprite SPR_BP_FARM_DOVE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Dove".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_DOVE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Dove".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Goat
        public readonly static Sprite SPR_BP_HEAD_GOAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goat".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_GOAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goat".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_GOAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goat".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_GOAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goat".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_GOAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goat".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Goose
        public readonly static Sprite SPR_BP_HEAD_GOOSE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goose".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_GOOSE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goose".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_GOOSE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Goose".LoadFromSheet("Spr_Bp_nLeg");
        #endregion

        #region Horse
        public readonly static Sprite SPR_BP_HEAD_HORSE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Horse".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_HORSE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Horse".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_HORSE
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Horse".LoadFromSheet("Spr_Bp_nLeg");
        #endregion

        #region Ladybug
        public readonly static Sprite SPR_BP_FLEG_LADYBUG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_LadyBug".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_LADYBUG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_LadyBug".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_LADYBUG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_LadyBug".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_LADYBUG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_LadyBug".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Pig
        public readonly static Sprite SPR_BP_FLEG_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_TORSO_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_HEAD_PIG
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Pig".LoadFromSheet("Spr_Bp_Head");
        #endregion

        #region Raccoon
        public readonly static Sprite SPR_BP_TORSO_RACCOON
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raccoon".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_FLEG_RACCOON
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raccoon".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_RACCOON
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raccoon".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_RACCOON
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raccoon".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_RACCOON
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raccoon".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Rat
        public readonly static Sprite SPR_BP_FLEG_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_RAT
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rat".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Raven
        public readonly static Sprite SPR_BP_TORSO_RAVEN
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raven".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_FLEG_RAVEN
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raven".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_RAVEN
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raven".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_RAVEN
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raven".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_RAVEN
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Raven".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Rooster
        public readonly static Sprite SPR_BP_TORSO_ROOSTER
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rooster".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_HEAD_ROOSTER
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rooster".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_ROOSTER
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rooster".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_ROOSTER
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rooster".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_ROOSTER
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rooster".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_ROOSTER
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Rooster".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #region Sheep
        public readonly static Sprite SPR_BP_TORSO_SHEEP
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Sheep".LoadFromSheet("Spr_Bp_Torso");
        public readonly static Sprite SPR_BP_HEAD_SHEEP
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Sheep".LoadFromSheet("Spr_Bp_Head");
        public readonly static Sprite SPR_BP_FLEG_SHEEP
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Sheep".LoadFromSheet("Spr_Bp_fLeg");
        public readonly static Sprite SPR_BP_NLEG_SHEEP
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Sheep".LoadFromSheet("Spr_Bp_nLeg");
        public readonly static Sprite SPR_BP_FARM_SHEEP
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Sheep".LoadFromSheet("Spr_Bp_fArm");
        public readonly static Sprite SPR_BP_NARM_SHEEP
            = "Art/BodyParts/Sprites/Spr_Sheet_BodyParts_Sheep".LoadFromSheet("Spr_Bp_nArm");
        #endregion

        #endregion


        #region Abilities

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

        #region Bear
        public readonly static Sprite SPR_UI_ABT_BEAR_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_bear-head");
        public readonly static Sprite SPR_UI_ABT_BEAR_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_bear-arm");
        #endregion

        #region Bee
        public readonly static Sprite SPR_UI_ABT_BEE_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_bee-head");
        public readonly static Sprite SPR_UI_ABT_BEE_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_bee-arm");
        #endregion

        #region Cat
        public readonly static Sprite SPR_UI_ABT_CAT_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cat-head");
        public readonly static Sprite SPR_UI_ABT_CAT_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cat-arm");
        #endregion

        #region Cockroach
        public readonly static Sprite SPR_UI_ABT_COCKROACH_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cockroach-head");
        public readonly static Sprite SPR_UI_ABT_COCKROACH_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cockroach-arm");
        #endregion

        #region Cow
        public readonly static Sprite SPR_UI_ABT_COW_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cow-head");
        public readonly static Sprite SPR_UI_ABT_COW_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_cow-arm");
        #endregion

        #region Dog
        public readonly static Sprite SPR_UI_ABT_DOG_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_dog-head");
        public readonly static Sprite SPR_UI_ABT_DOG_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_dog-arm");
        #endregion

        #region Dove
        public readonly static Sprite SPR_UI_ABT_DOVE_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_dove-head");
        public readonly static Sprite SPR_UI_ABT_DOVE_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_dove-arm");
        #endregion

        #region Goat
        public readonly static Sprite SPR_UI_ABT_GOAT_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_goat-head");
        public readonly static Sprite SPR_UI_ABT_GOAT_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_goat-arm");
        #endregion

        #region Goose
        public readonly static Sprite SPR_UI_ABT_GOOSE_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_goose-head");
        public readonly static Sprite SPR_UI_ABT_GOOSE_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_goose-arm");
        #endregion

        #region Horse
        public readonly static Sprite SPR_UI_ABT_HORSE_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_horse-head");
        public readonly static Sprite SPR_UI_ABT_HORSE_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_horse-arm");
        #endregion

        #region Ladybug
        public readonly static Sprite SPR_UI_ABT_LADYBUG_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_ladybug-head");
        public readonly static Sprite SPR_UI_ABT_LADYBUG_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_ladybug-arm");
        #endregion

        #region Pig
        public readonly static Sprite SPR_UI_ABT_PIG_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_pig-head");
        public readonly static Sprite SPR_UI_ABT_PIG_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_pig-arm");
        #endregion

        #region Raccoon
        public readonly static Sprite SPR_UI_ABT_RACCOON_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_raccoon-head");
        public readonly static Sprite SPR_UI_ABT_RACCOON_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_raccoon-arm");
        #endregion

        #region Rat
        public readonly static Sprite SPR_UI_ABT_RAT_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_rat-head");
        public readonly static Sprite SPR_UI_ABT_RAT_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_rat-arm");
        #endregion

        #region Raven
        public readonly static Sprite SPR_UI_ABT_RAVEN_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_raven-head");
        public readonly static Sprite SPR_UI_ABT_RAVEN_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_raven-arm");
        #endregion

        #region Rooster
        public readonly static Sprite SPR_UI_ABT_ROOSTER_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_rooster-head");
        public readonly static Sprite SPR_UI_ABT_ROOSTER_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_rooster-arm");
        #endregion

        #region Sheep
        public readonly static Sprite SPR_UI_ABT_SHEEP_HEAD
            = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_sheep-head");
        public readonly static Sprite SPR_UI_ABT_SHEEP_ARM
           = "Art/Abilities/Spr_Sheet_Abilities".LoadFromSheet("spr_sheep-arm");
        #endregion

        #endregion
    }
}
