using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class BearArmEffect : IDbRecord
    {
        public BearArmEffect()
        {
            ID("effect_bear-arm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 6
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 1
            });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public sealed class BearLegEffect : IDbRecord
    {
        public BearLegEffect()
        {
            ID("effect_bear-leg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 6
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 1
            });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
