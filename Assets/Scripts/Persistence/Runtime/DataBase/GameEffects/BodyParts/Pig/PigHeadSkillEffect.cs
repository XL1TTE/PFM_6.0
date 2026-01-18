using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class PigHeadSkillEffect : IDbRecord
    {
        public PigHeadSkillEffect()
        {
            ID("effect_pig-head-skill");

            With<Name>(new Name("PigHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 7
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 5
            });
        }
    }
}
