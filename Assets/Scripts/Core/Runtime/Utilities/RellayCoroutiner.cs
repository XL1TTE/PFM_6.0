using System.Collections;
using UnityEngine;

namespace Core.Utilities
{
    
    public class RellayCoroutiner: MonoBehaviour{
        
        private static RellayCoroutiner _instance;

        void Awake()
        {
            if(_instance == null){
                _instance = this;
            }
        }

        public static Coroutine Run(IEnumerator enumerator){
            if(_instance == null){return null;}
            return _instance.StartCoroutine(enumerator);
        }
        
        public static void Stop(Coroutine coroutine){
            if(_instance == null){return;}
            _instance.StopCoroutine(coroutine);
        }
    }
    
}
