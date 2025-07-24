using System;
using UnityEngine;

namespace UnityUtilities{
    
    [Serializable]
    public class Sprite: MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        
        
        /* ############################################## */
        /*                      COLOR                     */
        /* ############################################## */
        public Color GetColor() => _renderer.color;
        public void SetColor(Color color){
            if(_renderer is null){return;}
            
            _renderer.color = color;
        }
    }
    
}
