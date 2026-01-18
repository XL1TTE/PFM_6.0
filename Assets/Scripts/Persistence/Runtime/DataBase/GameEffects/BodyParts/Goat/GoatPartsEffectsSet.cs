using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class GoatHeadEffect : IDbRecord
    {
        public GoatHeadEffect()
        {
            ID("effect_goat-head");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 6
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 2
            });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public sealed class GoatArmEffect : IDbRecord
    {
        public GoatArmEffect()
        {
            ID("effect_goat-arm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 3
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
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public sealed class GoatLegEffect : IDbRecord
    {
        public GoatLegEffect()
        {
            ID("effect_goat-leg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 3
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
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
