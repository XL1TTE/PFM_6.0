using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public struct ApplyPoison : IApplyStatusEffect
    {
        public ApplyPoison(int a_duration, int a_damagePerTick)
        {
            this.m_Duration = a_duration;
            this.m_DamagePerTick = a_damagePerTick;
        }

        public int m_Duration { get; set; }
        public int m_DamagePerTick { get; set; }

        public IAbilityNode Clone() => new ApplyPoison(m_Duration, m_DamagePerTick);

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            G.Statuses.ApplyPoison(t_caster, t_target, m_Duration, m_DamagePerTick, t_world);

            return UniTask.CompletedTask;
        }
    }

}
