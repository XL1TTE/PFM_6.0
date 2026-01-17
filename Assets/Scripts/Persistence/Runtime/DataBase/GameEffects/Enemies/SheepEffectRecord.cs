using Domain.Stats.Components;

namespace Persistence.DB
{
    public class SheepEffectRecord : BodyPartRecord
    {
        public SheepEffectRecord()
        {
            ID("effect_Sheep");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 27
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 13
                });
        }
    }
}
