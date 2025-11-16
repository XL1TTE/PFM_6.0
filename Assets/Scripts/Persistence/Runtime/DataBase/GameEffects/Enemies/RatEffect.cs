using Domain.Stats.Components;

namespace Persistence.DB
{
    public class RatEffectRecord : MonsterPartRecord
    {
        public RatEffectRecord()
        {
            ID("effect_Rat");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 13
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
            With<BurningResistanceModiffier>(new BurningResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public class CowEffectRecord : MonsterPartRecord
    {
        public CowEffectRecord()
        {
            ID("effect_Cow");

            With<MaxHealthModifier>(
                new MaxHealthModifier
                {
                    m_Flat = 30
                });
            With<SpeedModifier>(
                new SpeedModifier
                {
                    m_Flat = 7
                });
        }
    }
}
