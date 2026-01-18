using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class GoatHeadSkillEffect : IDbRecord
    {
        public GoatHeadSkillEffect()
        {
            ID("effect_goat-head-skill");

            With<Name>(new Name("GoatHeadSkillEffect_name"));

            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = -5
            });
        }
    }
}
