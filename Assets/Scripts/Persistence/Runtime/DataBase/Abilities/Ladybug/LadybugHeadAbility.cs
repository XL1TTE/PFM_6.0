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
    public sealed class LadybugHeadAbility : IDbRecord
    {
        public LadybugHeadAbility()
        {
            ID("abt_ladybug-head");

            With<Name>(new Name("LadybugHeadAbility_name"));
            With<Description>(new Description("LadybugHeadAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_LADYBUG_HEAD));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DEBUFF
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[4]
                {
                     new Vector2Int(1, -1),
                     new Vector2Int(1, 1),
                     new Vector2Int(-1, 1),
                     new Vector2Int(-1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyToAllEnemiesInArea(new List<IAbilityNode>{
                        new ApplyEffect(2, "effect_ladybug-head-skill"),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
