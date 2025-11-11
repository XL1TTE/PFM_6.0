using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public struct ApplyBleeding : IAbilityNode
    {
        public ApplyBleeding(int a_duration, int a_damagePerTick)
        {
            this.m_Duration = a_duration;
            this.m_DamagePerTick = a_damagePerTick;
        }

        private int m_Duration;
        private int m_DamagePerTick;

        public IAbilityNode Clone() => new ApplyBleeding(m_Duration, m_DamagePerTick);

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            G.Statuses.ApplyBleeding(t_caster, t_target, m_Duration, m_DamagePerTick, t_world);

            return UniTask.CompletedTask;
        }
    }

}
