using Core.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Abilities;
using Domain.Services;
using Scellecs.Morpeh;
using Unity.VisualScripting;

namespace Gameplay.Abilities
{

    public class PlayTweenAnimation : IAbilityNode
    {
        private readonly TweenAnimations m_Animation;

        public PlayTweenAnimation(TweenAnimations a_animation)
        {
            m_Animation = a_animation;
        }

        public IAbilityNode Clone() => new PlayTweenAnimation(m_Animation);

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_attacker = context.m_Caster;
            var t_world = context.m_World;

            ResetAnimationContextState(context);

            t_world.GetRequest<AnimateWithTweenRequest>().Publish(new AnimateWithTweenRequest
            {
                m_Subject = t_attacker,
                m_Target = t_target,
                m_TweenAnimationCode = m_Animation
            });

            UpdateAnimationStateInContext(context).Forget();
            await UniTask.CompletedTask;
        }

        private async UniTask UpdateAnimationStateInContext(AbilityContext context)
        {
            var subject = context.m_Caster;
            var world = context.m_World;

            var stash_animationState = world.GetStash<AnimatingState>();

            await UniTask.Yield(PlayerLoopTiming.Update);

            if (stash_animationState.Has(subject) == false)
            {
                return;
            }
            WaitForTweenActionFrame(context, stash_animationState).Forget();

            WaitForAnimationOver(context, stash_animationState).Forget();
        }

        private async UniTask WaitForAnimationOver(AbilityContext context, Stash<AnimatingState> stash_State)
        {
            var subject = context.m_Caster;
            await UniTask.WaitUntil(() => IsAnimationOver(subject, stash_State));
            context.m_AnimationContext.m_IsAnimationOver = true;
        }

        private async UniTask WaitForTweenActionFrame(AbilityContext context, Stash<AnimatingState> stash_State)
        {
            var subject = context.m_Caster;
            await UniTask.WaitUntil(() => stash_State.Get(subject).m_IsTweenInteractionFrame);

            context.m_AnimationContext.m_IsTweenInteractionFrame = true;
        }

        private bool IsAnimationOver(Entity subject, Stash<AnimatingState> stash_State)
        {
            if (stash_State.Has(subject) == false) { return true; }

            ref var state = ref stash_State.Get(subject);

            if (state.m_Status == AnimatingStatus.COMPLETED || state.m_Status == AnimatingStatus.FAILED)
            {
                return true;
            }
            return false;
        }


        private void ResetAnimationContextState(AbilityContext context)
        {
            context.m_AnimationContext.m_IsAnimationOver = false;
            context.m_AnimationContext.m_IsTweenInteractionFrame = false;
        }
    }
}
