using Domain.Stats.Components;

namespace Persistence.DB
{
    public class SheepEffectRecord : BodyPartRecord
    {
        public SheepEffectRecord()
        {
            ID("effect_Sheep");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 8
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 13
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
