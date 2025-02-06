using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectGridLayoutSystem))]
public class ObjectGridLayoutSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectGridLayoutSystem gridSystem = (ObjectGridLayoutSystem)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Grid"))
        {
            gridSystem.GenerateGrid();
            EditorUtility.SetDirty(gridSystem);
        }

        if (GUILayout.Button("Clear Grid"))
        {
            gridSystem.ClearGrid();
            EditorUtility.SetDirty(gridSystem);
        }

        GUILayout.Space(10);
        GUILayout.Label("Generated Grid:", EditorStyles.boldLabel);

        GameObject[,] gridObjects = gridSystem.GetGridObjects();

        if (gridObjects != null)
        {
            int columns = gridObjects.GetLength(0);
            int rows = gridObjects.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < columns; x++)
                {
                    EditorGUILayout.ObjectField(gridObjects[x, y], typeof(GameObject), true, GUILayout.Width(60));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
