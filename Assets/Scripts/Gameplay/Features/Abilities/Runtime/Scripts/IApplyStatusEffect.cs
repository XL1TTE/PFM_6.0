using Domain.Abilities;

namespace Gameplay.Abilities
{
    public interface IApplyStatusEffect : IAbilityNode
    {
        int m_Duration { get; }
        int m_DamagePerTick { get; }
    }

}
