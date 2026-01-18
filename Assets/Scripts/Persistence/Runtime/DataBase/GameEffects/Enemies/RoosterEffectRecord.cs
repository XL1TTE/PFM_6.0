using Domain.Stats.Components;

namespace Persistence.DB
{
    public class RoosterEffectRecord : BodyPartRecord
    {
        public RoosterEffectRecord()
        {
            ID("effect_rooster");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 20
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 11
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
                m_Stage = IResistanceModiffier.Stage.RESISTANT
            });
        }
    }
}
