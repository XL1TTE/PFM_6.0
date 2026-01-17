using Domain.Stats.Components;

namespace Persistence.DB
{
    public class BearEffectRecord : BodyPartRecord
    {
        public BearEffectRecord()
        {
            ID("effect_bear");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 32
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 7
                });

            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.NONE
            });
            With<BleedResistanceModiffier>(new BleedResistanceModiffier
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
