using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public struct ApplyBurning : IApplyDamageStatusEffect
    {
        public ApplyBurning(int a_duration, int a_damagePerTick)
        {
            this.m_Duration = a_duration;
            this.m_DamagePerTick = a_damagePerTick;
        }

        public int m_Duration { get; set; }
        public int m_DamagePerTick { get; set; }

        public IAbilityNode Clone() => new ApplyBurning(m_Duration, m_DamagePerTick);

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            G.Statuses.ApplyBurning(t_caster, t_target, m_Duration, m_DamagePerTick, t_world);

            return UniTask.CompletedTask;
        }
    }

}
