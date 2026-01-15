
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class DogHeadAbility : IDbRecord
    {
        public DogHeadAbility()
        {
            ID("abt_dog-head");

            With<Name>(new Name("DogHeadAbility_name"));
            With<Description>(new Description("DogHeadAbility_desc"));

            //With<AbilityShiftsSprite>(new AbilityShiftsSprite() { m_Value = Resources.Load<Sprite>("Assets/Resources/Art/Abilities/Spr_Bodypart_Head_Test_1") });

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_COCKROACH_RADIOACTIVE));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE,
                    AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3]
                {
                     new Vector2Int(1, 0),
                     new Vector2Int(1, 1),
                     new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(Domain.Services.TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(4, DamageType.PHYSICAL_DAMAGE),
                    new ApplyEffect(2, "effect_dog-skill2"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
