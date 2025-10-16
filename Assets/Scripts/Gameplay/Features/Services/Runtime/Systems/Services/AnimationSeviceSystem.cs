using System;
using Core.Utilities;
using DG.Tweening;
using Domain.AbilityGraph;
using Domain.Components;
using Domain.Services;
using Domain.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

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
        private Event<TweenInteractionFrameRiched> evt_TweenInteractionFrameRiched;
        private Stash<TransformRefComponent> stash_TransformRef;

        public World World { get; set; }


        public void OnAwake()
        {
            req_Animating = World.GetRequest<AnimatingRequest>();
            req_AnimateWithTween = World.GetRequest<AnimateWithTweenRequest>();

            evt_AnimatingStarted = World.GetEvent<AnimatingStarted>();
            evt_AnimatingEnded = World.GetEvent<AnimatingEnded>();
            evt_TweenInteractionFrameRiched = World.GetEvent<TweenInteractionFrameRiched>();

            stash_TransformRef = World.GetStash<TransformRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_Animating.Consume())
            {

            }
            foreach (var req in req_AnimateWithTween.Consume())
            {
                if (stash_TransformRef.Has(req.m_Subject) == false) { OnAnimationFailed(req.m_Subject); }
                if (stash_TransformRef.Has(req.m_Target) == false) { OnAnimationFailed(req.m_Subject); }

                var subjectTransform = stash_TransformRef.Get(req.m_Subject).Value;
                var targetTransform = stash_TransformRef.Get(req.m_Target).Value;

                var animation = AbilityAnimations.GetStandartAttack(subjectTransform, targetTransform,
                     () => OnTweenInteractionFrame(req.m_Subject));

                animation.AppendCallback(() => OnAnimationSuccses(req.m_Subject));

                evt_AnimatingStarted.NextFrame(new AnimatingStarted
                {
                    m_Subject = req.m_Subject
                });
                animation.Play();
            }
        }

        private void OnTweenInteractionFrame(Entity subject)
        {
            evt_TweenInteractionFrameRiched.NextFrame(new TweenInteractionFrameRiched
            {
                m_Subject = subject
            });
        }

        private void OnAnimationFailed(Entity subject)
        {
            evt_AnimatingEnded.NextFrame(new AnimatingEnded
            {
                m_Subject = subject,
                m_AnimatingStatus = AnimatingStatus.FAILED
            });
        }

        private void OnAnimationSuccses(Entity subject)
        {
            evt_AnimatingEnded.NextFrame(new AnimatingEnded
            {
                m_Subject = subject,
                m_AnimatingStatus = AnimatingStatus.SUCCSESSED
            });
        }

        public void Dispose()
        {

        }
    }
}
