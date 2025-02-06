using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public ScriptableShapeInfo shapeInfo;

    public void OnDropOnCell(Cell targetCell, ObjectGridLayoutSystem gridSystem, Vector2Int cellPosition)
    {
        if (targetCell != null)
        {
            ApplyShape(targetCell, gridSystem, cellPosition);
        }
    }

    private void ApplyShape(Cell targetCell, ObjectGridLayoutSystem gridSystem, Vector2Int currentPos)
    {
        bool up = false, down = false, left = false, right = false;

        foreach (Direction dir in shapeInfo.centerCellDirections)
        {
            switch (dir)
            {
                case Direction.Up: up = true; break;
                case Direction.Down: down = true; break;
                case Direction.Left: left = true; break;
                case Direction.Right: right = true; break;
            }
        }

        targetCell.ActivateDirections(up, down, left, right);
        ActivateOppositeDirection(gridSystem, currentPos, up, down, left, right);

        foreach (NeighborConnection connection in shapeInfo.neighborConnections)
        {
            Vector2Int neighborPos = currentPos + connection.offset;

            if (IsWithinGrid(neighborPos, gridSystem))
            {
                Cell neighborCell = gridSystem.gridObjects[neighborPos.x, neighborPos.y].GetComponent<Cell>();

                bool neighborUp = false, neighborDown = false, neighborLeft = false, neighborRight = false;

                foreach (Direction dir in connection.activeDirections)
                {
                    switch (dir)
                    {
                        case Direction.Up: neighborUp = true; break;
                        case Direction.Down: neighborDown = true; break;
                        case Direction.Left: neighborLeft = true; break;
                        case Direction.Right: neighborRight = true; break;
                    }
                }

                neighborCell.ActivateDirections(neighborUp, neighborDown, neighborLeft, neighborRight);
                ActivateOppositeDirection(gridSystem, neighborPos, neighborUp, neighborDown, neighborLeft, neighborRight);
            }
        }
    }

    private void ActivateOppositeDirection(ObjectGridLayoutSystem gridSystem, Vector2Int pos, bool up, bool down, bool left, bool right)
    {
        if (up && IsWithinGrid(pos + Vector2Int.down, gridSystem))
            gridSystem.gridObjects[pos.x, pos.y - 1].GetComponent<Cell>().ActivateDirections(false, true, false, false);

        if (down && IsWithinGrid(pos + Vector2Int.up, gridSystem))
            gridSystem.gridObjects[pos.x, pos.y + 1].GetComponent<Cell>().ActivateDirections(true, false, false, false);

        if (left && IsWithinGrid(pos + Vector2Int.left, gridSystem))
            gridSystem.gridObjects[pos.x - 1, pos.y].GetComponent<Cell>().ActivateDirections(false, false, false, true);

        if (right && IsWithinGrid(pos + Vector2Int.right, gridSystem))
            gridSystem.gridObjects[pos.x + 1, pos.y].GetComponent<Cell>().ActivateDirections(false, false, true, false);
    }

    private bool IsWithinGrid(Vector2Int pos, ObjectGridLayoutSystem gridSystem)
    {
        return pos.x >= 0 && pos.y >= 0 &&
               pos.x < gridSystem.columns &&
               pos.y < gridSystem.rows;
    }
}
