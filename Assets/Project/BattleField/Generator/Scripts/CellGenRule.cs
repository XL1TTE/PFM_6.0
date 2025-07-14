using System;
using System.Net;
using Project.Domain;
using Scellecs.Morpeh;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleField.Generator{
    public class CellGenRule : MonoBehaviour{
        [HideInInspector] public Vector2Int GridPosition;
        [HideInInspector] public Vector3 WorldPosition;
        [SerializeField] public GameObject CellPrefab;
        [HideInInspector] public GameObject CellCache;
    
        [HideInInspector] private Entity CellEntityCache;

        void Start()
        {
            CellEntityCache = Domain.BattleField.CreateCellEntity(WorldPosition, GridPosition);
        }

        void OnDisable()
        {
            Domain.DeleteEntity(CellEntityCache);
        }

        public void UpdateCell()
        {
            if (this == null) { return; }

            DestroyCell();
            
            if(CellPrefab != null){
                
                CellCache = Instantiate(CellPrefab, transform.position, transform.rotation, transform);
                CellCache.name = "Cell";
                CellCache.hideFlags = HideFlags.NotEditable;
            }
        }

        private void OnDestroy()
        {
            DestroyCell();
        }

        private void DestroyCell() {
            
            if (CellCache != null)
            {
                if (Application.isPlaying)
                    Destroy(CellCache);
                else
                    DestroyImmediate(CellCache);
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
