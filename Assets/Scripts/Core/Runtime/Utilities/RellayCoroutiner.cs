using System.Collections;
using UnityEngine;

namespace UnityUtilities{
    
    public class RellayCoroutiner: MonoBehaviour{
        
        private static RellayCoroutiner _instance;

        void Awake()
        {
            if(_instance == null){
                _instance = this;
            }
        }

        public static Coroutine RellayCoroutine(IEnumerator enumerator){
            if(_instance == null){return null;}
            return _instance.StartCoroutine(enumerator);
        }
        
        public static void StopRellayCoroutine(Coroutine coroutine){
            if(_instance == null){return;}
            _instance.StopCoroutine(coroutine);
        }
    }
    
}
