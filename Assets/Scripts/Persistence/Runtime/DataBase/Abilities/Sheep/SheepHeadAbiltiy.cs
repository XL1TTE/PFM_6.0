
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
    public sealed class SheepHeadAbiltiy : IDbRecord
    {
        public SheepHeadAbiltiy()
        {
            ID("abt_sheep-head");

            With<Name>(new Name("SheepHeadAbiltiy_name"));
            With<Description>(new Description("SheepHeadAbiltiy_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_SHEEP_HEAD));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.HEAL
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ALLY,
                m_Shifts = new Vector2Int[5]
                {
                    new Vector2Int(2, 0),
                    new Vector2Int(2, 1),
                    new Vector2Int(2, -1),
                    new Vector2Int(2, 2),
                    new Vector2Int(2, -2),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new Heal(3),
                    new ApplyEffect(2, "effect_sheep-head-skill"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }

}
