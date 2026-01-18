using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class RoosterHeadSkillEffect : IDbRecord
    {
        public RoosterHeadSkillEffect()
        {
            ID("effect_rooster-head-skill");

            With<Name>(new Name("RoosterHeadSkillEffect_name"));

            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 5
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
