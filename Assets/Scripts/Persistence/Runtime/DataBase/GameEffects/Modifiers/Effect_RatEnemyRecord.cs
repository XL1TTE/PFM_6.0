using Domain.Components;
using Domain.Stats.Components;

namespace Persistence.DB
{
    public class Effect_RatEnemyRecord : MonsterPartRecord
    {
        public Effect_RatEnemyRecord()
        {
            ID("effect_RatEnemy");

            With<ID>(new ID { Value = "effect_RatEnemy" });
            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 5
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 2
                });
        }
    }
}
