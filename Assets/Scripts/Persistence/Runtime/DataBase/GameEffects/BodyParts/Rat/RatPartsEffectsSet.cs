using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class RatArmEffect : IDbRecord
    {
        public RatArmEffect()
        {
            ID("effect_RatArm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 4
            });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public sealed class RatLegEffect : IDbRecord
    {
        public RatLegEffect()
        {
            ID("effect_RatLeg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 4
            });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
