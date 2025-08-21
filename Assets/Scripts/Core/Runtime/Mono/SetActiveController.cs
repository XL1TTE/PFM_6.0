
using UnityEngine;

namespace Core.Mono
{
    public class SetActiveController : MonoBehaviour
    {
        
        [SerializeField] private GameObject RootObject;
        
        public void SetActive(bool isEnable){
            RootObject.SetActive(isEnable);
        }
    }
}
