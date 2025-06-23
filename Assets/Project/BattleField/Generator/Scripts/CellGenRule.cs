using System;
using System.Net;
using UnityEngine;

namespace BattleField.Generator{
    public class CellGenRule : MonoBehaviour{
        [HideInInspector] public Vector2Int CellPosition;
        [SerializeField] public GameObject CellPrefab;
        [HideInInspector] public GameObject Cell;

        public void UpdateCell()
        {
            if (this == null) { return; }

            DestroyCell();
            
            if(CellPrefab != null){
                Cell = Instantiate(CellPrefab, transform.position, transform.rotation, transform);
                Cell.name = "Cell";
                Cell.hideFlags = HideFlags.NotEditable;
            }
        }

        private void OnDestroy()
        {
            DestroyCell();
        }

        private void DestroyCell() {
            
            if (Cell != null)
            {
                if (Application.isPlaying)
                    Destroy(Cell);
                else
                    DestroyImmediate(Cell);
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector3 center = transform.position;
            Vector3 size = Vector3.one;

            Gizmos.DrawWireCube(center, size);

            string label = $"Cell Type: {(CellPrefab != null ? CellPrefab.name : "None")}";
            UnityEditor.Handles.Label(center + Vector3.up, label, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = Color.green },
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            });
        }
#endif

    }
}
