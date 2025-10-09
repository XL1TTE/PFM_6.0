using System;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI.Widgets
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider m_slider;

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
    }
}
