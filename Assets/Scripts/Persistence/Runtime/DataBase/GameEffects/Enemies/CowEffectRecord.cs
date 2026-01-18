using Domain.Stats.Components;

namespace Persistence.DB
{
    public class CowEffectRecord : BodyPartRecord
    {
        public CowEffectRecord()
        {
            ID("effect_Cow");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 16
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 7
                });
        }
    }
}
