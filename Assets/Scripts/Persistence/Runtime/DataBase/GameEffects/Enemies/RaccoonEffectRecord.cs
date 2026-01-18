using Domain.Stats.Components;

namespace Persistence.DB
{
    public class RaccoonEffectRecord : BodyPartRecord
    {
        public RaccoonEffectRecord()
        {
            ID("effect_raccoon");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 7
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 16
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
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
