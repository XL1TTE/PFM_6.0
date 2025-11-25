
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
    public sealed class CockroachArmAbility : IDbRecord
    {
        public CockroachArmAbility()
        {
            ID("abt_cockroach-arm");

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_COCKROACH_SPIT));

            With<Name>(new Name("Cockroach spit"));
            With<Description>(new Description("Spits acid that deals damage on contact to all enemies within a radius, and also poisons them."));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DEBUFF
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] {
                    new Vector2Int(1, 0),
                    new Vector2Int(2, 0),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyToAllEnemiesInArea(new List<IAbilityNode>{
                        new DealDamage(2, DamageType.POISON_DAMAGE),
                        new ApplyPoison(2, 5),
                    }, 1),

                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
