using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ScriptableShapeInfo))]
public class ScriptableShapeInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScriptableShapeInfo shapeInfo = (ScriptableShapeInfo)target;

        EditorGUILayout.LabelField("Center Cell Directions", EditorStyles.boldLabel);
        DrawDirectionToggles(shapeInfo.centerCellDirections);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Neighbor Connections", EditorStyles.boldLabel);

        if (shapeInfo.neighborConnections == null)
            shapeInfo.neighborConnections = new List<NeighborConnection>();

        int removeIndex = -1;
        for (int i = 0; i < shapeInfo.neighborConnections.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            NeighborConnection connection = shapeInfo.neighborConnections[i];

            connection.offset = EditorGUILayout.Vector2IntField("Neighbor Offset", connection.offset);
            DrawDirectionToggles(connection.activeDirections);

            if (GUILayout.Button("Remove Connection"))
                removeIndex = i;

            EditorGUILayout.EndVertical();
        }

        if (removeIndex >= 0)
            shapeInfo.neighborConnections.RemoveAt(removeIndex);

        if (GUILayout.Button("Add New Neighbor Connection"))
            shapeInfo.neighborConnections.Add(new NeighborConnection { activeDirections = new List<Direction>() });

        EditorUtility.SetDirty(shapeInfo);
    }

    private void DrawDirectionToggles(List<Direction> directions)
    {
        foreach (Direction dir in (Direction[])System.Enum.GetValues(typeof(Direction)))
        {
            if (dir == Direction.None) continue;

            bool isActive = directions.Contains(dir);
            bool toggle = EditorGUILayout.Toggle(dir.ToString(), isActive);

            if (toggle && !isActive)
                directions.Add(dir);
            else if (!toggle && isActive)
                directions.Remove(dir);
        }
    }
}
