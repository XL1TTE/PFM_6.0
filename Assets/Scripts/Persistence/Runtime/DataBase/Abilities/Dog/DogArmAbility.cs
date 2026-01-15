
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
    public sealed class DogArmAbility : IDbRecord
    {
        public DogArmAbility()
        {
            ID("abt_dog-arm");

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_COCKROACH_SPIT));

            With<Name>(new Name("DogArmAbility_name"));
            With<Description>(new Description("DogArmAbility_desc"));


            With<AbilityShiftsSprite>(new AbilityShiftsSprite() { m_Value = Resources.Load<Sprite>("Assets/Resources/Art/Abilities/Spr_Bodypart_Head_Test_1") });

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
