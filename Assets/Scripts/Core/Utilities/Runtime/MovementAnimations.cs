using DG.Tweening;
using Domain.Components;
using Domain.Extentions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Core.Utilities
{
    public static class MovementAnimations
    {
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
