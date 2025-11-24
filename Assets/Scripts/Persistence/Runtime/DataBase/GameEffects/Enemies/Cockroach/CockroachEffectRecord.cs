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
                    m_Flat = 10
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 12
                });
        }
    }
}
