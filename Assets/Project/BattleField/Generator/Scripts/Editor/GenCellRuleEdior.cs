
using UnityEditor;
using UnityEngine;

namespace BattleField.Generator.Editor{
    #if UNITY_EDITOR
    
    [CustomEditor(typeof(CellGenRule))]
    public class CellGenRuleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CellPrefab"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                ((CellGenRule)target).UpdateCell();
            }

            if (GUILayout.Button("Update Cell"))
            {
                ((CellGenRule)target).UpdateCell();
            }
        }
    }
#endif
}
