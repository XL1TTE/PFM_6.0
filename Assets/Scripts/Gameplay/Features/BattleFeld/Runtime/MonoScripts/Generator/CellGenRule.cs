using Core.Utilities.Extentions;
using Gameplay.Features.Monster.Data;
using Infrastructure.Data;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Gameplay.Features.BattleField
{
    public class CellGenRule : MonoBehaviour{
        [HideInInspector] public Vector2Int GridPosition;
        [SerializeField] public GameObject CellPrefab;
        [SerializeField] public GameObject CellCache;
    
        [HideInInspector] private Entity CellEntityCache;

        void Start()
        {   
            if(CellCache == null || !CellCache.TryFindComponent<EntityProvider>(out var unknown)){return;}
            Entity entity =  unknown.Entity;
            
            CellEntityCache = BattleFieldEntitiesBuilder.SetupCellEntity(entity, gameObject.transform.position, GridPosition);
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
