using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public class TurnAround : IAbilityNode
    {
        public IAbilityNode Clone() => new TurnAround();

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            await G.TurnAroundAsync(t_caster, t_world);
        }
    }

}
