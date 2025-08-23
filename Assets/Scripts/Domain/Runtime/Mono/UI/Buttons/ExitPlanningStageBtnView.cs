using UnityEngine;

namespace Domain.UI.Mono
{
    public class ExitPlanningStageBtnView : MonoBehaviour
    {
        
        [SerializeField] GameObject RootButtonObject;
        
        public void DestroySelf(){
            Destroy(RootButtonObject);
        }
    }
}
