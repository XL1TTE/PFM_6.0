using Domain.Components;
using Domain.Stats.Components;

namespace Persistence.DB
{
    public class Effects_DinHeadRecord : MonsterPartRecord
    {
        public Effects_DinHeadRecord()
        {
            ID("effect_DinHead");

            With<ID>(new ID { Value = "effect_DinHead" });
            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 10
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 5
            });
        }
    }
}
