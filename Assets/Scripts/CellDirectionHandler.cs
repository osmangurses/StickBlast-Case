public class CellDirectionHandler
{
    private Cell cell;

    public CellDirectionHandler(Cell cell)
    {
        this.cell = cell;
    }

    public void ActivateDirections(bool up, bool down, bool left, bool right)
    {
        if (up) cell.up = up;
        if (down) cell.down = down;
        if (left) cell.left = left;
        if (right) cell.right = right;
    }

    public void DeactivateDir(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                cell.up = false;
                break;
            case Direction.Down:
                cell.down = false;
                break;
            case Direction.Left:
                cell.left = false;
                break;
            case Direction.Right:
                cell.right = false;
                break;
        }
    }

    public void DeactivateAll()
    {
        cell.up = cell.down = cell.left = cell.right = false;
    }

    public bool IsAllDirActivated()
    {
        return cell.up && cell.down && cell.left && cell.right;
    }
}
