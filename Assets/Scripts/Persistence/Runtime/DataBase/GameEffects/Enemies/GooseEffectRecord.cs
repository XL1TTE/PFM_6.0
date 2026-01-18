using Domain.Stats.Components;

namespace Persistence.DB
{
    public class GooseEffectRecord : BodyPartRecord
    {
        public GooseEffectRecord()
        {
            ID("effect_goose");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 23
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 9
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
