using System;
using UnityEngine;

namespace Core.Utilities.Wrappers
{
    [Serializable]
    public class Sprite: MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        
        [SerializeField] private Color _defaultColor;

        void Awake()
        {
            _defaultColor = _renderer.color;
        }

        /* ############################################## */
        /*                      COLOR                     */
        /* ############################################## */
        public Color GetColor() => _renderer.color;
        public Color GetDefaultColor() => _defaultColor;
        public void SetColor(Color color){
            if(_renderer is null){return;}
            
            _renderer.color = color;
        }
    }
    
}
