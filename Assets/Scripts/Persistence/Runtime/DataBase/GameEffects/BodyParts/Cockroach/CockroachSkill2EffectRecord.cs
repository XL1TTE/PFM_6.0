using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public class CockroachSkill2EffectRecord : BodyPartRecord
    {
        public CockroachSkill2EffectRecord()
        {
            ID("effect_cockroach-skill2");

            With<Name>(new Name("CockroachSkill2EffectRecord_name"));

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = -5
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = -3
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.STRONG_WEAKNESS
            });
        }
    }
}
