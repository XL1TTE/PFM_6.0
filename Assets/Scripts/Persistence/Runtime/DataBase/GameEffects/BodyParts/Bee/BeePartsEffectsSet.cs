using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class BeeTorsoEffect : IDbRecord
    {
        public BeeTorsoEffect()
        {
            ID("effect_bee-torso");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 5
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 2
            });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
    public sealed class BeeArmEffect : IDbRecord
    {
        public BeeArmEffect()
        {
            ID("effect_bee-arm");

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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
