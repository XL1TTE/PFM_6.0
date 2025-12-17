using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public struct ApplyStun : IApplyNonDamageStatusEffect
    {
        public ApplyStun(int a_duration)
        {
            this.m_Duration = a_duration;
        }

        public int m_Duration { get; set; }

        public IAbilityNode Clone() => new ApplyStun(m_Duration);

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            G.Statuses.ApplyStun(t_caster, t_target, m_Duration, t_world);

            return UniTask.CompletedTask;
        }
    }

}
