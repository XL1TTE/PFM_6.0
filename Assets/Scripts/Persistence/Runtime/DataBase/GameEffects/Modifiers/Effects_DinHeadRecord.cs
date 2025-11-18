using Domain.Components;
using Domain.Stats.Components;

namespace Persistence.DB
{
    public class Effects_DinHeadRecord : BodyPartRecord
    {
        public Effects_DinHeadRecord()
        {
            ID("effect_DinHead");

            With<ID>(new ID { m_Value = "effect_DinHead" });
            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 32,
                m_Multiplier = 0.1f
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 5
            });
        }
    }
}
