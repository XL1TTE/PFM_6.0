using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class GooseArmSkillEffect : IDbRecord
    {
        public GooseArmSkillEffect()
        {
            ID("effect_goose-arm-skill");

            With<Name>(new Name("GooseArmSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = -2
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = -2
            });
        }
    }
}
