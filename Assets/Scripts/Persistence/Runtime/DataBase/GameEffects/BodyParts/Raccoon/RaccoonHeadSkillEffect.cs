using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class RaccoonHeadSkillEffect : IDbRecord
    {
        public RaccoonHeadSkillEffect()
        {
            ID("effect_raccoon-head-skill");

            With<Name>(new Name("RaccoonHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = -4
            });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
        }
    }
}
