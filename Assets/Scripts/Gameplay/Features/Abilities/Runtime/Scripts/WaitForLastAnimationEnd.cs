using Cysharp.Threading.Tasks;
using Domain.Abilities;

namespace Gameplay.Abilities
{
    public struct WaitForLastAnimationEnd : IAbilityNode
    {
        public IAbilityNode Clone() => new WaitForLastAnimationEnd();

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            await UniTask.WaitUntil(()
                => context.m_AnimationContext.m_IsAnimationOver);
        }
    }
}
