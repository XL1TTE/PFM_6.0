using Domain.Stats.Components;

namespace Persistence.DB
{
    public sealed class PigArmSkillEffect : IDbRecord
    {
        public PigArmSkillEffect()
        {
            ID("effect_pig_skill1");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = -4
            });
        }
    }
}
