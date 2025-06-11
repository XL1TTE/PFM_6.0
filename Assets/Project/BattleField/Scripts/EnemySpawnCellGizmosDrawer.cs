using UnityEngine;

namespace BattleField.Cells
{
    public class EnemySpawnCellGizmosDrawer : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            Vector3 center = transform.position;
            Vector3 size = Vector3.one;
            Gizmos.DrawCube(center, size);
        }
#endif
    }
}
