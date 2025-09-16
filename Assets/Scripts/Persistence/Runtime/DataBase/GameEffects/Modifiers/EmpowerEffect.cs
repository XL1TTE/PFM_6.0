using Domain.Components;
using Domain.GameEffects.Modifiers;

namespace Persistence.DB
{
    public class EmpowerEffectRecord: MonsterPartRecord
    {
        public EmpowerEffectRecord()
        {
            With<ID>(new ID { Value = "effect_Empower" });
            With<MaxHealthModifier>(
                new MaxHealthModifier {
                    AdditiveValue = 3
                });
            With<MaxSpeedModifier>(new MaxSpeedModifier{
                AdditiveValue = 1
            });
        }
    }
}
