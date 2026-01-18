using Domain.Stats.Components;

namespace Persistence.DB
{
    public class CatEffectRecord : BodyPartRecord
    {
        public CatEffectRecord()
        {
            ID("effect_cat");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 7
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 14
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
}
