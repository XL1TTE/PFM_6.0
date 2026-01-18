using Domain.Stats.Components;

namespace Persistence.DB
{

    public sealed class SheepArmEffect : IDbRecord
    {
        public SheepArmEffect()
        {
            ID("effect_sheep-arm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 3
            });

            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class SheepLegEffect : IDbRecord
    {
        public SheepLegEffect()
        {
            ID("effect_sheep-leg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 3
            });
        }
    }

    public sealed class SheepHeadEffect : IDbRecord
    {
        public SheepHeadEffect()
        {
            ID("effect_sheep-head");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 5
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 1
            });

            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class SheepTorsoEffect : IDbRecord
    {
        public SheepTorsoEffect()
        {
            ID("effect_sheep-torso");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 5
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 2
            });

            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
}
