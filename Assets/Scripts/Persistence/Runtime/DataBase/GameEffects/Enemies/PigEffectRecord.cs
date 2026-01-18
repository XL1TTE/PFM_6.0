using Domain.Stats.Components;

namespace Persistence.DB
{
    public class PigEffectRecord : BodyPartRecord
    {
        public PigEffectRecord()
        {
            ID("effect_Pig");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 14
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 9
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
