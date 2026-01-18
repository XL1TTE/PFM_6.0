using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class DogHeadEffect : IDbRecord
    {
        public DogHeadEffect()
        {
            ID("effect_dog-head");

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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
    public sealed class DogTorsoEffect : IDbRecord
    {
        public DogTorsoEffect()
        {
            ID("effect_dog-torso");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 6
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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
    public sealed class DogLegEffect : IDbRecord
    {
        public DogLegEffect()
        {
            ID("effect_dog-leg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
