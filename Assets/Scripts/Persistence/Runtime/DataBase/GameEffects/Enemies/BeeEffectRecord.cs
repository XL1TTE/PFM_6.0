using Domain.Stats.Components;

namespace Persistence.DB
{
    public class BeeEffectRecord : BodyPartRecord
    {
        public BeeEffectRecord()
        {
            ID("effect_bee");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 18
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 12
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.RESISTANT
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
