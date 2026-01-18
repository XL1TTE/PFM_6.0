using Domain.Stats.Components;

namespace Persistence.DB
{
    public class CockroachEffectRecord : BodyPartRecord
    {
        public CockroachEffectRecord()
        {
            ID("effect_cockroach");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 5
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 12
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
