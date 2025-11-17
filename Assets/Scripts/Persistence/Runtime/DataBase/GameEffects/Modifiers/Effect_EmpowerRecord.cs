using Domain.Components;
using Domain.Stats.Components;

namespace Persistence.DB
{
    public partial class Effect_EmpowerRecord : BodyPartRecord
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
