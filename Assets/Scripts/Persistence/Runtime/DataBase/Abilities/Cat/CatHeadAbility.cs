using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Domain.Services;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class CAT : IDbRecord
    {
        public CAT()
        {
            ID("abt_cat-head");

            With<Name>(new Name("CatHeadAbility_name"));
            With<Description>(new Description("CatHeadAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_CAT_HEAD));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DEBUFF
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3]
                {
                     new Vector2Int(2, 1),
                     new Vector2Int(2, 0),
                     new Vector2Int(2, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyEffect(3, "effect_cat-head-skill"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
