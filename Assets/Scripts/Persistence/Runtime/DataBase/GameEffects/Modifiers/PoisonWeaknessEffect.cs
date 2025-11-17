using Domain.Stats.Components;

namespace Persistence.DB
{
    public partial class PoisonWeaknessEffect : MonsterPartRecord
    {
        public PoisonWeaknessEffect()
        {
            ID("effect_poison_weak");

            With<PoisonResistanceModiffier>(
                new PoisonResistanceModiffier
                {
                    m_Stage = IResistanceModiffier.Stage.WEAKNESS
                });
        }

    }
}
