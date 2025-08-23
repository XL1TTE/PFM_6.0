using UnityEngine;

namespace Domain.BattleField.Mono
{
    public class CellGizmosDrawer : MonoBehaviour
    {
        [SerializeField] private Color GizmosColor;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = GizmosColor;
            Vector3 center = transform.position;
            Vector3 size = Vector3.one;
            Gizmos.DrawCube(center, size);
        }
#endif
    }
}
