using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class PigHeadEffect : IDbRecord
    {
        public PigHeadEffect()
        {
            ID("effect_pig-head");

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
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public sealed class PigLegEffect : IDbRecord
    {
        public PigLegEffect()
        {
            ID("effect_pig-leg");

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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class PigTorsoEffect : IDbRecord
    {
        public PigTorsoEffect()
        {
            ID("effect_pig-torso");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 10
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 1
            });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
