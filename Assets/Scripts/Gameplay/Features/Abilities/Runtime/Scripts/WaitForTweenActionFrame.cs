using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;
using Interactions;

namespace Gameplay.Abilities
{
    public struct WaitForTweenActionFrame : IAbilityEffect
    {
        public IAbilityEffect Clone() => new WaitForTweenActionFrame();

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            await UniTask.WaitUntil(()
                => context.m_AnimationContext.m_IsTweenInteractionFrame
                | context.m_AnimationContext.m_IsAnimationOver);
        }
    }
}
