using Domain.Stats.Components;

namespace Persistence.DB
{
    public class Effect_MaxHealthDebuffRecord : IDbRecord
    {
        public Effect_MaxHealthDebuffRecord()
        {
            ID("effect_max_health_debuff");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = -2,
                });
        }
    }

}
