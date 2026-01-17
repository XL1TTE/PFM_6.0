using Domain.Stats.Components;

namespace Persistence.DB
{
    public class DogEffectRecord : BodyPartRecord
    {
        public DogEffectRecord()
        {
            ID("effect_dog");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 20
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 10
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
                m_Stage = IResistanceModiffier.Stage.NONE
            });
        }
    }
}
