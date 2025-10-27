using System;
using DG.Tweening;
using Domain.Components;
using Domain.Extentions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Core.Utilities
{
    public static class A
    {
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
        public static Sequence TurnAround(Transform subjectTransform)
        {
            Sequence seq = DOTween.Sequence();
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

            seq.Append(subject.DOMoveY(originalPosition.y
                + raiseHeight, 0.25f).SetEase(Ease.OutQuad));

            seq.Append(subject.DOMove(targetPosWithHeight, 0.5f)
                .SetEase(Ease.OutQuad));

            seq.Append(subject.DOMoveY(targetPos.y, 0.25f)
                .SetEase(Ease.InQuad));

            return seq;
        }
    }
}
