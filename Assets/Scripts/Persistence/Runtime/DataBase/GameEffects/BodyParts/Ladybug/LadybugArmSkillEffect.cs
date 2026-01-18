using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class LadybugArmSkillEffect : IDbRecord
    {
        public LadybugArmSkillEffect()
        {
            ID("effect_ladybug-arm-skill");

            With<Name>(new Name("LadybugHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 3
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 3
            });
        }
    }
}