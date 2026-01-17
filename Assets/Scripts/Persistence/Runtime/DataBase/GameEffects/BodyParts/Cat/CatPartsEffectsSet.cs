using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class CatHeadEffect : IDbRecord
    {
        public CatHeadEffect()
        {
            ID("effect_cat-head");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 3
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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class CatArmEffect : IDbRecord
    {
        public CatArmEffect()
        {
            ID("effect_cat-arm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
    public sealed class CatLegEffect : IDbRecord
    {
        public CatLegEffect()
        {
            ID("effect_cat-leg");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 2
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
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
