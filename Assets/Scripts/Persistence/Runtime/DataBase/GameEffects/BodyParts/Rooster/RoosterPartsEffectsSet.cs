using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class RoosterHeadEffect : IDbRecord
    {
        public RoosterHeadEffect()
        {
            ID("effect_rooster-head");

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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class RoosterTorsoEffect : IDbRecord
    {
        public RoosterTorsoEffect()
        {
            ID("effect_rooster-torso");

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
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class RoosterArmEffect : IDbRecord
    {
        public RoosterArmEffect()
        {
            ID("effect_rooster-arm");

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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class RoosterLegEffect : IDbRecord
    {
        public RoosterLegEffect()
        {
            ID("effect_rooster-leg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 3
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 3
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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
}
