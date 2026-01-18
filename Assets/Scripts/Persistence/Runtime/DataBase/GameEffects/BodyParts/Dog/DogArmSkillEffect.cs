using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class DogArmSkillEffect : IDbRecord
    {
        public DogArmSkillEffect()
        {
            ID("effect_dog-arm-skill");

            With<Name>(new Name("DogHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 8
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 2
            });
        }
    }
}
