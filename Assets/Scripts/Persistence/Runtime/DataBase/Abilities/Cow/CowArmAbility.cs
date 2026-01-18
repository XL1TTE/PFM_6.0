
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Domain.Services;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class CowArmAbility : IDbRecord
    {
        public CowArmAbility()
        {
            ID("abt_cow-arm");

            With<Name>(new Name("CowArmAbility_name"));
            With<Description>(new Description("CowArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_COW_ARM));

            With<AbilityShiftsSprite>(new AbilityShiftsSprite() { m_Value = Resources.Load<Sprite>("Assets/Resources/Art/Abilities/Spr_Bodypart_Head_Test_1") });

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[4]
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1),
                    new Vector2Int(0, 2),
                    new Vector2Int(0, -2),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(3, DamageType.PHYSICAL_DAMAGE),
                    new ApplyStun(2),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
