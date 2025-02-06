using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

[System.Serializable]
public class NeighborConnection
{
    public Vector2Int offset;
    public List<Direction> activeDirections;
}

[CreateAssetMenu(menuName = "New Shape Info", fileName = "Shape_0x0", order = 0)]
public class ScriptableShapeInfo : ScriptableObject
{
    [Header("Center Cell Settings")]
    public List<Direction> centerCellDirections = new List<Direction>();

    [Header("Neighbor Connections")]
    public List<NeighborConnection> neighborConnections = new List<NeighborConnection>();
}
