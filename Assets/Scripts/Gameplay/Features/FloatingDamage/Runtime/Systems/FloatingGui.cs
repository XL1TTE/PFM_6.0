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
            var spawnPoint = Camera.main.WorldToScreenPoint(a_spawnIn);

            var anim = m_instance.GetAnim(spawnPoint, a_element);

            anim.onComplete += () => a_element.Reset();

            anim.Play();
        }

        public void Dispose()
        {

        }

        [SerializeField] Ease m_FloatingEase = Ease.OutCubic;

        [SerializeField] float m_FloatingHeight = 100f;
        [SerializeField] float m_FloatingDuration = 4f;
        [SerializeField] float m_StartDisappearingOnSec = 2f;
        [SerializeField] float m_DisappearDuration = 0.25f;
        [SerializeField] Ease m_DisappearEase = Ease.OutCubic;

        private Sequence GetAnim(Vector3 spawnPoint, IUIElement a_element)
        {
            Sequence floatSequence = DOTween.Sequence();

            Vector3 endPosition = spawnPoint + new Vector3(0, m_FloatingHeight, 0);

            floatSequence.Append(a_element.gameObject.transform.DOMove(spawnPoint, 0f));
            floatSequence.AppendCallback(() => a_element.gameObject.SetActive(true));

            floatSequence.Append(a_element.gameObject.transform.DOMove(endPosition, m_FloatingDuration)
                .SetEase(m_FloatingEase));


            floatSequence.Insert(m_StartDisappearingOnSec, a_element.transform.DOScale(0, m_DisappearDuration))
                .SetEase(m_DisappearEase);

            return floatSequence;
        }
    }
}
