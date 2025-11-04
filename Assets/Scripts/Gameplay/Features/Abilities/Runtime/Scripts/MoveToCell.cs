using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public class MoveToCell : IAbilityNode
    {
        public IAbilityNode Clone() => new MoveToCell();

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            await G.MoveToCellAsync(t_caster, t_target, t_world);
        }
    }

}
