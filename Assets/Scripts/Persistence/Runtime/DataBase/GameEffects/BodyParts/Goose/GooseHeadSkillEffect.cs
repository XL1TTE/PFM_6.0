using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class GooseHeadSkillEffect : IDbRecord
    {
        public GooseHeadSkillEffect()
        {
            ID("effect_goose-head-skill");

            With<Name>(new Name("GooseHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = -5
            });
        }
    }
}
