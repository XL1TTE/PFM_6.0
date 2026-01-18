using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public sealed class HorseHeadSkillEffect : IDbRecord
    {
        public HorseHeadSkillEffect()
        {
            ID("effect_horse-head-skill");

            With<Name>(new Name("HorseHeadSkillEffect_name"));

            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
        }
    }
}
