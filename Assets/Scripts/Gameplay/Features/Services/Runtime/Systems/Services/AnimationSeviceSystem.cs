using System;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Components;
using Domain.Services;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Commands
{

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AnimationSeviceSystem : ISystem
    {
        private Request<AnimatingRequest> req_Animating;
        private Request<AnimateWithTweenRequest> req_AnimateWithTween;
        private Event<AnimatingStarted> evt_AnimatingStarted;
        private Event<AnimatingEnded> evt_AnimatingEnded;
        private Stash<AnimatingState> stash_AnimatingState;
        private Stash<TransformRefComponent> stash_TransformRef;

        public World World { get; set; }

        private IntHashMap<Sequence> m_ActiveTweensMap = new();

        public void OnAwake()
        {
            req_Animating = World.GetRequest<AnimatingRequest>();
            req_AnimateWithTween = World.GetRequest<AnimateWithTweenRequest>();

            evt_AnimatingStarted = World.GetEvent<AnimatingStarted>();
            evt_AnimatingEnded = World.GetEvent<AnimatingEnded>();

            stash_AnimatingState = World.GetStash<AnimatingState>();
            stash_TransformRef = World.GetStash<TransformRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_Animating.Consume())
            {

            }
            foreach (var req in req_AnimateWithTween.Consume())
            {
                stash_AnimatingState.Set(req.m_Subject);
                ref var state = ref stash_AnimatingState.Get(req.m_Subject);

                if (stash_TransformRef.Has(req.m_Subject) == false) { OnAnimationFailed(ref state, req.m_Subject); }
                if (stash_TransformRef.Has(req.m_Target) == false) { OnAnimationFailed(ref state, req.m_Subject); }

                KillAllActiveAnimationsFor(req.m_Subject);

                Sequence animation;
                switch (req.m_TweenAnimationCode)
                {
                    case TweenAnimations.ATTACK:
                        animation = PlayAttackAnimation(ref state, req);
                        m_ActiveTweensMap.Add(req.m_Subject.Id, animation, out var _);
                        break;
                    case TweenAnimations.TURN_AROUND:
                        animation = PlayTurnAroundAnimation(ref state, req);
                        m_ActiveTweensMap.Add(req.m_Subject.Id, animation, out var _);

                        break;
                }
            }
        }

        private void KillAllActiveAnimationsFor(Entity a_subject)
        {
            if (m_ActiveTweensMap.Has(a_subject.Id))
            {
                m_ActiveTweensMap.Remove(a_subject.Id, out var anim);
                anim?.Kill(true);
            }
        }

        private Sequence PlayTurnAroundAnimation(ref AnimatingState state, AnimateWithTweenRequest req)
        {
            var subjectTransform = stash_TransformRef.Get(req.m_Subject).Value;

            var animation = A.TurnAround(subjectTransform);

            animation.AppendCallback(() => OnAnimationCompleted(req.m_Subject));

            state.m_Status = AnimatingStatus.IN_PROCESS;
            evt_AnimatingStarted.NextFrame(new AnimatingStarted
            {
                m_Subject = req.m_Subject
            });
            animation.Play();
            return animation;
        }

        private Sequence PlayAttackAnimation(ref AnimatingState state, AnimateWithTweenRequest req)
        {
            var subjectTransform = stash_TransformRef.Get(req.m_Subject).Value;
            var targetTransform = stash_TransformRef.Get(req.m_Target).Value;

            var animation = A.StandartAttack(subjectTransform, targetTransform,
                 () => OnTweenInteractionFrame(req.m_Subject));

            animation.AppendCallback(() => OnAnimationCompleted(req.m_Subject));

            state.m_Status = AnimatingStatus.IN_PROCESS;
            evt_AnimatingStarted.NextFrame(new AnimatingStarted
            {
                m_Subject = req.m_Subject
            });
            animation.Play();

            return animation;
        }

        private void OnTweenInteractionFrame(Entity subject)
        {
            ref var state = ref stash_AnimatingState.Get(subject);
            state.m_IsTweenInteractionFrame = true;
        }

        private void OnAnimationFailed(ref AnimatingState state, Entity subject)
        {
            evt_AnimatingEnded.NextFrame(new AnimatingEnded
            {
                m_Subject = subject,
                m_AnimatingStatus = AnimatingStatus.FAILED
            });
        }

        private void OnAnimationCompleted(Entity subject)
        {
            ref var state = ref stash_AnimatingState.Get(subject);
            state.m_Status = AnimatingStatus.COMPLETED;

            RemoveAnimatingStateAsync(subject).Forget();

            evt_AnimatingEnded.NextFrame(new AnimatingEnded
            {
                m_Subject = subject,
                m_AnimatingStatus = AnimatingStatus.COMPLETED
            });
        }

        private async UniTask RemoveAnimatingStateAsync(Entity subject)
        {
            await UniTask.Yield();
            stash_AnimatingState.Remove(subject);
        }

        public void Dispose()
        {

        }
    }
}
