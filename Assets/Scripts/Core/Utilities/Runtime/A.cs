using System;
using System.Collections.Generic;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.Components;
using Domain.Extentions;
using Domain.FloatingDamage;
using Persistence.DB;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace Core.Utilities
{
    public static class A
    {
        private static IntHashMap<Sequence> m_SequencesCacheMap = new IntHashMap<Sequence>();

        /// <summary>
        /// Caches DOtween sequnce animation for entity.
        /// Can only store one sequnce at the time, so 
        /// if any animation already exist for entity, it will be killed and replaced.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sequence"></param>
        public static void CacheSequenceFor(Entity entity, Sequence sequence)
        {
            if (m_SequencesCacheMap.Has(entity.Id))
            {
                m_SequencesCacheMap.Remove(entity.Id, out var lastValue);
                lastValue?.Kill();
                lastValue = null;
            }
            m_SequencesCacheMap.Add(entity.Id, sequence, out var slotIndex);
        }

        /// <summary>
        /// Kills sequence on entity if it exists.
        /// </summary>
        /// <param name="entity"></param>
        public static void KillSequenceFor(Entity entity)
        {
            if (m_SequencesCacheMap.Has(entity.Id))
            {
                m_SequencesCacheMap.Remove(entity.Id, out var lastValue);
                lastValue?.Kill();
                lastValue = null;
            }
        }

        public static Sequence StandartAttack(Entity a_attacker, Entity t_target, World a_world, Action onDamageFrame = default)
        {
            var isAttackerWithTransform
                = a_world.TryGetComponent<TransformRefComponent>(a_attacker, out var attacker_transform);
            var isTargretWithTransform
                = a_world.TryGetComponent<TransformRefComponent>(t_target, out var target_transform);

            if (isAttackerWithTransform && isTargretWithTransform)
            {
                return A.StandartAttack(attacker_transform.Value, target_transform.Value, onDamageFrame);
            }
            return null;
        }

        public static Sequence StandartAttack(Transform attackerTransform, Transform targetTransform, Action onDamageFrame = default)
        {
            var raiseHeight = 20;

            Sequence seq = DOTween.Sequence();
            seq.SetLink(attackerTransform.gameObject);

            var originalPosition = attackerTransform.position;
            var targetPos = targetTransform.position;

            seq.Append(attackerTransform.DOMoveZ(originalPosition.z - 1.0f, 0.1f));

            seq.Append(attackerTransform.DOMoveY(originalPosition.y
                + raiseHeight, 0.5f).SetEase(Ease.OutQuad));

            seq.Append(attackerTransform.DOMove(targetPos, 0.25f)
                .SetEase(Ease.InExpo));

            seq.AppendCallback(() => onDamageFrame?.Invoke());

            var attackPos = new Vector3(originalPosition.x,
                originalPosition.y + raiseHeight, originalPosition.z - 1.0f);

            seq.Append(attackerTransform.DOMove(attackPos, 0.25f)
                .SetEase(Ease.OutQuad));

            seq.Append(attackerTransform.DOMove(originalPosition, 0.5f)
                .SetEase(Ease.InQuad));


            return seq;
        }


        public static Sequence TurnAround(Entity a_subject, World a_world)
        {
            var stash_TransformRef = a_world.GetStash<TransformRefComponent>();
            if (stash_TransformRef.Has(a_subject) == false) { return DOTween.Sequence(); }

            return TurnAround(stash_TransformRef.Get(a_subject).Value);
        }
        public static Sequence TurnAround(Transform subjectTransform)
        {
            Sequence seq = DOTween.Sequence();
            seq.SetLink(subjectTransform.gameObject);
            seq.Append(subjectTransform.DORotate(new Vector3(0, 180, 0), 0.25f, RotateMode.LocalAxisAdd));

            return seq;
        }


        public static Sequence ChessMovement(
            Entity subject,
            Entity target,
            World world,
            float raiseHeight = 20.0f
        )
        {
            if (!world.TryGetComponent<TransformRefComponent>(subject, out var subject_transform))
            {
                return DOTween.Sequence();
            }
            if (!world.TryGetComponent<TransformRefComponent>(target, out var target_transform))
            {
                return DOTween.Sequence();
            }

            return ChessMovement(subject_transform.Value, target_transform.Value.position, raiseHeight);
        }

        private static Sequence ChessMovement(
            Transform subject, Vector3 targetPos, float raiseHeight = 20.0f)
        {
            var targetPosWithHeight =
                new Vector3(targetPos.x, targetPos.y + raiseHeight, targetPos.z);

            var originalPosition = subject.position;

            Sequence seq = DOTween.Sequence();
            seq.SetLink(subject.gameObject);

            seq.Append(subject.DOMoveY(originalPosition.y
                + raiseHeight, 0.25f).SetEase(Ease.OutQuad));

            seq.Append(subject.DOMove(targetPosWithHeight, 0.5f)
                .SetEase(Ease.OutQuad));

            seq.Append(subject.DOMoveY(targetPos.y, 0.25f)
                .SetEase(Ease.InQuad));

            return seq;
        }


        public static Sequence Die(Entity a_subject, float a_duration, World a_world)
        {
            var stash_SpriteRenderer = a_world.GetStash<SpriteRendererComponent>();
            if (stash_SpriteRenderer.Has(a_subject) == false)
            {
                return DOTween.Sequence();
            }
            ref var t_renderer = ref stash_SpriteRenderer.Get(a_subject).m_SpriteRenderer;

            return Die(t_renderer, a_duration);
        }

        public static Sequence Die(SpriteRenderer a_renderer, float a_duration)
        {
            var BLINK_COUNT = 3;
            var BLINK_DURATION = BLINK_COUNT / a_duration;
            var INITIAL_COLOR = a_renderer.color;

            var FADE_DURATION = 1;

            Sequence deathSequence = DOTween.Sequence();
            deathSequence.SetLink(a_renderer.gameObject);

            for (int i = 0; i < BLINK_COUNT; ++i)
            {
                deathSequence.Append(a_renderer.DOColor(new Color(1, 1, 1, 0.3f), BLINK_DURATION / 2));
                deathSequence.Append(a_renderer.DOColor(INITIAL_COLOR, BLINK_DURATION / 2));
            }

            deathSequence.Append(a_renderer.DOFade(0f, FADE_DURATION));

            return deathSequence;
        }

    }
}
