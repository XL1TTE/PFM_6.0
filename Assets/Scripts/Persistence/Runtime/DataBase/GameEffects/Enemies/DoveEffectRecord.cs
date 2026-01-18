using Domain.Stats.Components;

namespace Persistence.DB
{
    public class DoveEffectRecord : BodyPartRecord
    {
        public DoveEffectRecord()
        {
            ID("effect_dove");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 6
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 16
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
