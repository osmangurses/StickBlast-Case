using System.Threading.Tasks;
using UnityEngine;
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
        cell.allDirActivated = false;
        RunDelayedCheckNeighborDirection();

    }
    private async void RunDelayedCheckNeighborDirection()
    {
        await Task.Delay(10);
        CheckNeighborDirection();
    }
    private void CheckNeighborDirection()
    {
        var neighborUp = cell.GetNeighborCell(new Vector2Int(0, -1));
        var neighborDown = cell.GetNeighborCell(new Vector2Int(0, 1));
        var neighborLeft = cell.GetNeighborCell(new Vector2Int(-1, 0));
        var neighborRight = cell.GetNeighborCell(new Vector2Int(1, 0));

        if (neighborUp != null && neighborUp.allDirActivated) cell.up = true;
        if (neighborDown != null && neighborDown.allDirActivated) cell.down = true;
        if (neighborLeft != null && neighborLeft.allDirActivated) cell.left = true;
        if (neighborRight != null && neighborRight.allDirActivated) cell.right = true;
    }

    public bool IsAllDirActivated()
    {
        return cell.up && cell.down && cell.left && cell.right;
    }
}
