using Domain.Stats.Components;

namespace Persistence.DB
{
    public class RavenEffectRecord : BodyPartRecord
    {
        public RavenEffectRecord()
        {
            ID("effect_raven");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 26
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 8
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
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
