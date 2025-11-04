using System;
using System.Collections.Generic;
using DG.Tweening;
using Domain.AbilityGraph;
using Domain.Components;
using Domain.Events;
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
    public sealed class FloatingDamageSystem : ISystem
    {
        public World World { get; set; }


        private Event<DamageDealtEvent> evt_OnDamageDealt;
        private Stash<TransformRefComponent> stash_TransformRef;
        private Transform m_PoolContainer;
        private Queue<FloatingDamageView> m_Pool = new();


        public void OnAwake()
        {
            evt_OnDamageDealt = World.GetEvent<DamageDealtEvent>();

            stash_TransformRef = World.GetStash<TransformRefComponent>();

            InitializePool();
        }

        private void InitializePool()
        {
            m_PoolContainer = new GameObject("[FlOATING_DAMAGE]").transform;

        }

        public void OnUpdate(float deltaTime)
        {
            ProcessDamageDealt();
        }

        private void ProcessDamageDealt()
        {
            foreach (var evt in evt_OnDamageDealt.publishedChanges)
            {
                if (stash_TransformRef.Has(evt.m_Target) == false) { continue; }

                ref var spawnPoint = ref stash_TransformRef.Get(evt.m_Target).Value;

                FloatingDamageView floatingDamage;

                if (m_Pool.Count <= 0)
                {
                    floatingDamage = CreateFloatingDamage();
                }
                else
                {
                    floatingDamage = m_Pool.Dequeue();
                }

                floatingDamage.Value.text = evt.m_FinalDamage.ToString();
                var anim = GetFloatingDamageAnim(spawnPoint, floatingDamage);

                anim.onComplete += () => ReturnToPool(floatingDamage);

                anim.Play();
            }
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
