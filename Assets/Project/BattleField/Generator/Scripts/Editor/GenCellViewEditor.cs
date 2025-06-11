#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BattleField.Generator.Editor{

    [CustomEditor(typeof(GenCellView))]
    public class GenCellViewEditor : UnityEditor.Editor
    {
        private static readonly Color[] typeColors = new Color[]
        {
            Color.white, 
            Color.green,
            Color.red,
        };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GenCellView cellView = (GenCellView)target;
            UpdateCellAppearance(cellView);
        }

        private void OnSceneGUI()
        {
            GenCellView cellView = (GenCellView)target;
            UpdateCellAppearance(cellView);
        }

        private void UpdateCellAppearance(GenCellView cellView)
        {
            cellView.TryGetComponent<SpriteRenderer>(out var renderer);
            if(renderer == null){
                renderer = cellView.GetComponentInChildren<SpriteRenderer>();
            }
            
            if (renderer != null)
            {
                int typeIndex = (int)cellView.FieldType;
                if (typeIndex >= 0 && typeIndex < typeColors.Length)
                {
                    renderer.color = typeColors[typeIndex];
                    EditorUtility.SetDirty(renderer);
                }
            }
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            GenCellView[] allCells = FindObjectsByType<GenCellView>(FindObjectsSortMode.None);
            foreach (var cell in allCells)
            {
                cell.TryGetComponent<SpriteRenderer>(out var renderer);
                if(renderer == null){
                    renderer = cell.GetComponentInChildren<SpriteRenderer>();
                }
                
                if (renderer != null)
                {
                    int typeIndex = (int)cell.FieldType;
                    if (typeIndex >= 0 && typeIndex < typeColors.Length)
                    {
                        renderer.color = typeColors[typeIndex];
                    }
                }
            }
        }
    }
#endif
}
