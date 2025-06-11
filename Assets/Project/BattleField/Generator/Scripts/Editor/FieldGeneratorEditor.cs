using UnityEditor;
using UnityEngine;

namespace BattleField.Generator.Editor{

    [CustomEditor(typeof(BattleGridGenerator))]
    public class BattleGridGeneratorEditor : UnityEditor.Editor
    {
        private BattleGridGenerator generator;
        private SerializedProperty gridData;

        private void OnEnable()
        {
            generator = (BattleGridGenerator)target;
            gridData = serializedObject.FindProperty("_inspectorGrid");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            if (GUILayout.Button("Generate Grid"))
            {
                generator.GenerateGrid();
            }

            if (GUILayout.Button("Clear Grid"))
            {
                generator.ClearGrid();
            }

            EditorGUILayout.Space();
            if (generator.EditMode)
            {
                EditorGUILayout.HelpBox("Click on cells below to select/deselect", MessageType.Info);
                DrawGridEditor();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGridEditor()
        {
            if (generator._inspectorGrid == null) return;

            EditorGUILayout.LabelField("Grid Editor");

            int cols = Mathf.Min(10, generator.gridSize.x);
            int rows = Mathf.CeilToInt(generator.gridSize.y / (float)cols);

            for (int y = generator.gridSize.y - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();

                for (int x = 0; x < generator.gridSize.x; x++)
                {
                    var cell = generator._inspectorGrid[x, y];
                    string label = cell.isSelected ? "X" : "";

                    GUIStyle style = new GUIStyle(GUI.skin.button);
                    if (cell.isSelected)
                    {
                        style.normal.background = MakeTex(1, 1, Color.green);
                    }

                    if (GUILayout.Button(label, style, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        cell.isSelected = !cell.isSelected;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
