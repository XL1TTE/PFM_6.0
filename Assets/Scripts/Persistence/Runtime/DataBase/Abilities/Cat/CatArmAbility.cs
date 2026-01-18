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
    public sealed class CatArmAbility : IDbRecord
    {
        public CatArmAbility()
        {
            ID("abt_cat-arm");

            With<Name>(new Name("CatArmAbility_name"));
            With<Description>(new Description("CatArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_CAT_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] {
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(4, DamageType.PHYSICAL_DAMAGE),
                    new ApplyBleeding(2, 3),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
