using UnityEngine;

public class CellValidator
{
    private ObjectGridLayoutSystem gridSystem;

    public CellValidator(ObjectGridLayoutSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }

    public bool CheckCellDirs(Shape shape, Cell cell)
    {
        return !(shape.shapeInfo.centerCellDirections.Contains(Direction.Up) && cell.up ||
                 shape.shapeInfo.centerCellDirections.Contains(Direction.Down) && cell.down ||
                 shape.shapeInfo.centerCellDirections.Contains(Direction.Left) && cell.left ||
                 shape.shapeInfo.centerCellDirections.Contains(Direction.Right) && cell.right);
    }

    public Vector2Int GetCellPosition(Cell cell)
    {
        for (int x = 0; x < gridSystem.columns; x++)
        {
            for (int y = 0; y < gridSystem.rows; y++)
            {
                if (gridSystem.gridObjects[x, y].GetComponent<Cell>() == cell)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public bool CanPlaceAnyShape()
    {
        foreach (var gridObject in gridSystem.gridObjects)
        {
            Cell cell = gridObject.GetComponent<Cell>();
            if (cell == null) continue;
            if (ShapeSpawner.instance.spawnedShapes.Count == 0) return true;
            foreach (var shapeObject in ShapeSpawner.instance.spawnedShapes)
            {
                if (shapeObject == null) continue;
                Shape shape = shapeObject.GetComponent<Shape>();
                if (shape == null) continue;

                if (CheckCellDirs(shape, cell))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
