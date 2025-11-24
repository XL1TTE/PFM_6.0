using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class CowArmEffect : IDbRecord
    {
        public CowArmEffect()
        {
            ID("effect_CowArm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 5
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 1
            });
        }
    }
}
