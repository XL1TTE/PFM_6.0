using UnityEngine;

namespace UI.Mono.View
{
    public class ExitPlanningStageBtnView : MonoBehaviour
    {
        
        [SerializeField] GameObject RootButtonObject;
        
        public void DestroySelf(){
            Destroy(RootButtonObject);
        }
    }
}
