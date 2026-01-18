using Domain.Stats.Components;

namespace Persistence.DB
{
    public class GoatEffectRecord : BodyPartRecord
    {
        public GoatEffectRecord()
        {
            ID("effect_goat");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 9
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
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
}
