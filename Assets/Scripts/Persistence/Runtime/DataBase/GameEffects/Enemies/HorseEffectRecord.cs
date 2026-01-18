using Domain.Stats.Components;

namespace Persistence.DB
{
    public class HorseEffectRecord : BodyPartRecord
    {
        public HorseEffectRecord()
        {
            ID("effect_horse");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 27
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 8
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
}
