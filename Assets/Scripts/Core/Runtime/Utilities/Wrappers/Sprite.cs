using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Utilities.Wrappers
{
    [Serializable]
    public class Sprite: MonoBehaviour{
        
        [SerializeField] private SpriteRenderer _renderer;
        
        [HideInInspector] private Color _defaultColor;
        
        [HideInInspector] private UnityEngine.Sprite _previousSprite;
        
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
        
        public void SetSprite(UnityEngine.Sprite sprite){
            if(_renderer != null){
                _previousSprite = _renderer.sprite;
                _renderer.sprite = sprite;
            }
        }
    
        public UnityEngine.Sprite GetPreviousSprite() => _previousSprite;
    }
    
}
