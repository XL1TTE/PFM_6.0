using Domain.Stats.Components;

namespace Persistence.DB
{
    public class RatEffectRecord : BodyPartRecord
    {
        public RatEffectRecord()
        {
            ID("effect_Rat"); // Идентификатор эффекта.

            With<MaxHealthModifier>( // <-- Модификатор, который добавит 13 ед. здоровья.
                new MaxHealthModifier
                {
                    m_Flat = 13
                });
            With<SpeedModifier>( // <-- Добавляем 16 ед. скорости.
                new SpeedModifier
                {
                    m_Flat = 16
                });

            // Устанавливаем значение сопротивления к яду на иммунитет.
            With<PoisonResistanceModiffier>(new PoisonResistanceModiffier
            {
                m_Stage = IResistanceModiffier.Stage.IMMUNE
            });
        }
    }
    public class CowEffectRecord : BodyPartRecord
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
