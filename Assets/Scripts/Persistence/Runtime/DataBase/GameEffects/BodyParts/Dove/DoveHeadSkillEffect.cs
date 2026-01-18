using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class DoveHeadSkillEffect : IDbRecord
    {
        public DoveHeadSkillEffect()
        {
            ID("effect_dove-head-skill");

            With<Name>(new Name("DoveHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 3
            });
        }
    }
}
