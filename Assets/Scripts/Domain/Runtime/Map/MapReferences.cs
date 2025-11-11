using UnityEngine;

namespace Domain.Map
{
    public class MapReferences : MonoBehaviour
    {
        private static MapReferences _instance;

        void Awake()
        {
            if (_instance == null )
            {
                _instance = this;
            }
        }

        public static MapReferences Instance() => _instance;

        public Transform mainCameraContainer;

        public Camera mainCamera;

        public GameObject linesRef;
        public GameObject nodesRef;
        public GameObject bgsRef;


    }
}
