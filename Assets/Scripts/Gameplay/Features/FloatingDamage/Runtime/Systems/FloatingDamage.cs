using System;
using System.Collections.Generic;
using DG.Tweening;
using Domain.Abilities;
using Domain.Components;
using Domain.Extentions;
using Domain.FloatingDamage;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.FloatingDamage.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FloatingDamage : MonoBehaviour
    {
        [SerializeField] private Transform m_PoolContainer;
        private Queue<FloatingDamageView> m_Pool = new();

        private static FloatingDamage m_instance;
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
            if (m_PoolContainer == null)
            {
                m_PoolContainer = new GameObject("[FlOATING_DAMAGE]").transform;
            }
        }

        public static bool IsInstantiated() => m_instance != null;

        public static void Show(Entity a_target, int a_value, DamageType a_damageType, World a_world)
        {
            var stash_TransformRef = a_world.GetStash<TransformRefComponent>();

            if (stash_TransformRef.Has(a_target) == false) { return; }

            ref var spawnPoint = ref stash_TransformRef.Get(a_target).Value;

            FloatingDamageView floatingDamage;

            if (m_instance.m_Pool.Count <= 0)
            {
                floatingDamage = m_instance.CreateFloatingDamage();
            }
            else
            {
                floatingDamage = m_instance.m_Pool.Dequeue();
            }

            floatingDamage.Value.text = a_value.ToString();

            switch (a_damageType)
            {
                case DamageType.BLEED_DAMAGE:
                    floatingDamage.Value.color = Color.red;
                    break;
                case DamageType.PHYSICAL_DAMAGE:
                    floatingDamage.Value.color = Color.white;
                    break;
                case DamageType.FIRE_DAMAGE:
                    floatingDamage.Value.color = Color.yellow;
                    break;
                case DamageType.POISON_DAMAGE:
                    floatingDamage.Value.color = Color.green;
                    break;
            }

            var anim = m_instance.GetFloatingDamageAnim(spawnPoint, floatingDamage);

            anim.onComplete += () => m_instance.ReturnToPool(floatingDamage);

            anim.Play();
        }

        public void Dispose()
        {

        }

        private FloatingDamageView CreateFloatingDamage()
        {
            var floatingDamage = UnityEngine.Object.
                Instantiate(GR.p_FloatingDamage, m_PoolContainer);

            floatingDamage.gameObject.SetActive(false);
            return floatingDamage;
        }


        private void ReturnToPool(FloatingDamageView floatingDamage)
        {
            floatingDamage.Hide();
            m_Pool.Enqueue(floatingDamage);
        }

        private Sequence GetFloatingDamageAnim(Transform spawnPoint, FloatingDamageView floatingDamage)
        {
            var floatHeight = 100f;
            var floatDuration = 3f;

            Sequence floatSequence = DOTween.Sequence();

            Vector3 endPosition = spawnPoint.position +
                                 new Vector3(
                                     UnityEngine.Random.Range(-30f, 30f),
                                     floatHeight,
                                     UnityEngine.Random.Range(-30f, 30f)
                                 );

            floatSequence.Append(floatingDamage.gameObject.transform.DOMove(spawnPoint.position, 0f));
            floatSequence.AppendCallback(() => floatingDamage.Show());

            floatSequence.Append(floatingDamage.gameObject.transform.DOMove(endPosition, floatDuration)
                .SetEase(Ease.OutCubic));


            floatSequence.Insert(1f, floatingDamage.transform.DOScale(0, 2f)
                .SetEase(Ease.OutBack));

            floatSequence.onComplete += () => floatingDamage.ResetView();
            floatSequence.onComplete += () => floatingDamage.Hide();

            return floatSequence;
        }
    }
}
