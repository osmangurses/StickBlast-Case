public class ShapeDirectionHandler
{
    public void AdjustShapeDirection(Shape shape, Cell targetCell)
    {
        if (shape.shapeInfo.centerCellDirections.Contains(Direction.Right) &&
            !(shape.shapeInfo.centerCellDirections.Contains(Direction.Left) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Up) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Down)) &&
            shape.transform.position.x < targetCell.transform.position.x)
        {
            shape.shapeInfo.centerCellDirections.Remove(Direction.Right);
            shape.shapeInfo.centerCellDirections.Add(Direction.Left);
        }

        if (shape.shapeInfo.centerCellDirections.Contains(Direction.Left) &&
            !(shape.shapeInfo.centerCellDirections.Contains(Direction.Right) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Up) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Down)) &&
            shape.transform.position.x > targetCell.transform.position.x)
        {
            shape.shapeInfo.centerCellDirections.Remove(Direction.Left);
            shape.shapeInfo.centerCellDirections.Add(Direction.Right);
        }

        if (shape.shapeInfo.centerCellDirections.Contains(Direction.Up) &&
            !(shape.shapeInfo.centerCellDirections.Contains(Direction.Right) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Left) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Down)) &&
            shape.transform.position.y < targetCell.transform.position.y)
        {
            shape.shapeInfo.centerCellDirections.Remove(Direction.Up);
            shape.shapeInfo.centerCellDirections.Add(Direction.Down);
        }

        if (shape.shapeInfo.centerCellDirections.Contains(Direction.Down) &&
            !(shape.shapeInfo.centerCellDirections.Contains(Direction.Right) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Left) ||
              shape.shapeInfo.centerCellDirections.Contains(Direction.Up)) &&
            shape.transform.position.y > targetCell.transform.position.y)
        {
            shape.shapeInfo.centerCellDirections.Remove(Direction.Down);
            shape.shapeInfo.centerCellDirections.Add(Direction.Up);
        }
    }
}
