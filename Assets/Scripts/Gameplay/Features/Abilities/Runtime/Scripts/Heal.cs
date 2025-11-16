using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public class Heal : IAbilityNode
    {
        public int m_Amount;
        public Heal(int a_amount)
        {
            m_Amount = a_amount;
        }

        public IAbilityNode Clone()
        {
            return new Heal(m_Amount);
        }

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            G.Heal(t_caster, t_target, m_Amount, t_world);
            return UniTask.CompletedTask;
        }
    }

}
