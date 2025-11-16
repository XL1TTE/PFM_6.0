using DG.Tweening;
using UI.Elements;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.FloatingDamage.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FloatingGui : MonoBehaviour
    {
        private static FloatingGui m_instance;
        public void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public static bool IsInstantiated() => m_instance != null;

        public static void Show(Vector3 a_spawnIn, IUIElement a_element)
        {
            var spawnPoint = Camera.main.WorldToScreenPoint(a_spawnIn) + new Vector3(
                                     UnityEngine.Random.Range(-40f, 40f),
                                     UnityEngine.Random.Range(-20f, 20f),
                                     a_spawnIn.z
                                 );

            var anim = m_instance.GetAnim(spawnPoint, a_element);

            anim.onComplete += () => a_element.Reset();

            anim.Play();
        }

        public void Dispose()
        {

        }
        private Sequence GetAnim(Vector3 spawnPoint, IUIElement a_element)
        {
            var floatHeight = 100f;
            var floatDuration = 3f;

            Sequence floatSequence = DOTween.Sequence();

            Vector3 endPosition = spawnPoint +
                                 new Vector3(
                                     UnityEngine.Random.Range(-50f, 50f),
                                     floatHeight,
                                     spawnPoint.z
                                 );

            floatSequence.Append(a_element.gameObject.transform.DOMove(spawnPoint, 0f));
            floatSequence.AppendCallback(() => a_element.gameObject.SetActive(true));

            floatSequence.Append(a_element.gameObject.transform.DOMove(endPosition, floatDuration)
                .SetEase(Ease.OutCubic));


            floatSequence.Insert(1f, a_element.transform.DOScale(0, 2f)
                .SetEase(Ease.OutBack));

            return floatSequence;
        }
    }
}
