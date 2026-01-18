using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class RavenHeadSkillEffect : IDbRecord
    {
        public RavenHeadSkillEffect()
        {
            ID("effect_raven-head-skill");

            With<Name>(new Name("RavenHeadSkillEffect_name"));

            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = -3
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
        }
    }
}
