using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class CatHeadSkillEffect : IDbRecord
    {
        public CatHeadSkillEffect()
        {
            ID("effect_cat-head-skill");

            With<Name>(new Name("CatHeadSkillEffect_name"));

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = -3
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
        }
    }
}
