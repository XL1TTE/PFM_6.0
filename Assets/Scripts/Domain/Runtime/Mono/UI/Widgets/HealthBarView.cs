using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Scellecs.Morpeh.Collections;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider m_slider;
        [SerializeField] private Transform m_RootObject;
        [SerializeField] private Transform m_EffectsContainer;

        [SerializeField] private List<Icon> m_EffectIconsCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">percent from 0 to 1</param>
        public void SetValue(float value)
        {
            m_slider.value = Math.Clamp(value, 0.0f, 1.0f);
        }

        public void SetPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }

        public async UniTask AddStatusEffect(Sprite icon, IStatusEffectComponent.Stack stack)
        {
            var iconPrototype = IconPool.I().WarmupElement()
                .SetIcon(icon)
                .MinSize(16);

            iconPrototype.SetLayout(m_EffectsContainer);

            m_EffectIconsCache.Add(iconPrototype);

            await UniTask.Yield();

            iconPrototype.Show();
        }

        public void ClearStatuses()
        {
            for (int i = 0; i < m_EffectIconsCache.Count; ++i)
            {
                m_EffectIconsCache[i].Free();
            }
        }

        public void DestorySelf() => Destroy(m_RootObject.gameObject, 0.1f);
    }
}
