using Domain.Abilities;

namespace Gameplay.Abilities
{


    public interface IApplyDamageStatusEffect : IAbilityNode
    {
        int m_Duration { get; }
        int m_DamagePerTick { get; }
    }
    public interface IApplyNonDamageStatusEffect : IAbilityNode
    {
        int m_Duration { get; }
    }

}
