using Domain.Stats.Components;

namespace Persistence.DB
{
    public class LadybugEffectRecord : BodyPartRecord
    {
        public LadybugEffectRecord()
        {
            ID("effect_ladybug");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 8
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 16
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
