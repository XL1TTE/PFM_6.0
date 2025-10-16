using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Utilities
{
    public static class AbilityAnimations
    {
        public static Sequence GetStandartAttack(Transform attackerTransform, Transform targetTransform, Action onDamageFrame = default)
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
    }
}
