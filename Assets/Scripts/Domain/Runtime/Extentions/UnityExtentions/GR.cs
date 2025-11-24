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

        public readonly static Sprite SPR_UI_PHYSICAL_DMG = "Art/Spr_UI_Effect_Debuff".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_BLOOD = "Art/Spr_UI_Effect_Blood".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_FIRE = "Art/Spr_UI_Effect_Fire".LoadResource<Sprite>();
        public readonly static Sprite SPR_UI_EFFECT_POISON = "Art/Spr_UI_Effect_Poison".LoadResource<Sprite>();


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
    }
}
