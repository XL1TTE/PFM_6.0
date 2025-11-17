using Domain.Components;
using Domain.Stats.Components;

namespace Persistence.DB
{
    public class Effect_RatEnemyRecord : BodyPartRecord
    {
        public Effect_RatEnemyRecord()
        {
            ID("effect_RatEnemy");

            With<ID>(new ID { m_Value = "effect_RatEnemy" });
            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 16
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 2
                });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
