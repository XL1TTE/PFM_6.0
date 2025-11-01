using Domain.Abilities.Mono;
using Domain.FloatingDamage;
using Domain.UI.Widgets;
using UnityEngine;

namespace Domain.Extentions
{
    public static partial class GR
    {

        public readonly static HealthBarView p_MonsterHealthBar = "UI/MonsterHealthBar".LoadResource<HealthBarView>();
        public readonly static HealthBarView p_EnemyHealthBar = "UI/EnemyHealthBar".LoadResource<HealthBarView>();


        public readonly static FloatingDamageView p_FloatingDamage = "UI/FloatingDamage".LoadResource<FloatingDamageView>();


        public readonly static AbilityButtonView p_AbilityButton = "Abilities/AbilityButton".LoadResource<AbilityButtonView>();



        public readonly static Sprite SPR_EFFECT_ABILITY_ICON = "UI/Sprites/Abilities/Spr_Battle_UI_Icon_Effect".LoadResource<Sprite>();
        public readonly static Sprite SPR_ATTACK_ABILITY_ICON = "UI/Sprites/Abilities/Spr_Battle_UI_Icon_Attack".LoadResource<Sprite>();
        public readonly static Sprite SPR_MOVE_ABILITY_ICON = "UI/Sprites/Abilities/Spr_Battle_UI_Icon_Moving".LoadResource<Sprite>();
        public readonly static Sprite SPR_TURN_AROUND_ABILITY_ICON = "UI/Sprites/Abilities/Spr_Battle_UI_Icon_Turn".LoadResource<Sprite>();
    }
}
