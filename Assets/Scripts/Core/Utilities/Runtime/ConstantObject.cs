using UnityEngine;

namespace Core.Utilities
{
    public class ConstantObject : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
