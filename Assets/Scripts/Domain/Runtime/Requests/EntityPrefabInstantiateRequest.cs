using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.Requests
{
    public struct EntityPrefabInstantiateRequest : IRequestData{        
        public string GUID;
        public GameObject EntityPrefab;
        public Transform Parent;
    }
}
