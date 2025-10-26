using Domain.Components;
using Domain.Stats.Components;

namespace Persistence.DB
{
    public class Effect_EmpowerRecord : MonsterPartRecord
    {
        public Effect_EmpowerRecord()
        {
            ID("effect_Empower");

            With<ID>(new ID { m_Value = "effect_Empower" });
            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 3,
                    m_Multiplier = 0.25f
                });
        }
    }
}
