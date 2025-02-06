using System.Collections;
using UnityEngine;

public class CellBlastHandler
{
    private Cell cell;
    private ObjectGridLayoutSystem gridSystem;

    public CellBlastHandler(Cell cell, ObjectGridLayoutSystem gridSystem)
    {
        this.cell = cell;
        this.gridSystem = gridSystem;
    }

    public IEnumerator Blast()
    {
        bool hasHorizontal = CheckHorizontal();
        bool hasVertical = CheckVertical();

        yield return new WaitForSeconds(0.5f);

        if (hasHorizontal)
        {
            DeactivateRow();
        }
        if (hasVertical)
        {
            DeactivateColumn();
        }
    }



    public void HandleDeactivation()
    {
        foreach (var gridObject in gridSystem.gridObjects)
        {
            Cell otherCell = gridObject.GetComponent<Cell>();
            if (otherCell == null) continue;

            Vector2Int thisPos = GetCellPosition(cell);
            Vector2Int otherPos = GetCellPosition(otherCell);

            if (thisPos.x + 1 == otherPos.x && thisPos.y == otherPos.y)
            {
                otherCell.DeactivateDir(Direction.Left);
            }
            else if (thisPos.x - 1 == otherPos.x && thisPos.y == otherPos.y)
            {
                otherCell.DeactivateDir(Direction.Right);
            }
            else if (thisPos.y + 1 == otherPos.y && thisPos.x == otherPos.x)
            {
                otherCell.DeactivateDir(Direction.Up);
            }
            else if (thisPos.y - 1 == otherPos.y && thisPos.x == otherPos.x)
            {
                otherCell.DeactivateDir(Direction.Down);
            }
        }

        ParticlePlayer.instance.PlayParticle("Deactivate", cell.transform.position);
    }
    private void DeactivateRow()
    {
        foreach (var gridObject in gridSystem.gridObjects)
        {
            Cell otherCell = gridObject.GetComponent<Cell>();
            if (GetCellPosition(otherCell).x == GetCellPosition(cell).x)
            {
                otherCell.Deactivate();
            }
        }
    }

    private void DeactivateColumn()
    {
        foreach (var gridObject in gridSystem.gridObjects)
        {
            Cell otherCell = gridObject.GetComponent<Cell>();
            if (GetCellPosition(otherCell).y == GetCellPosition(cell).y)
            {
                otherCell.Deactivate();
            }
        }
    }


    private bool CheckHorizontal()
    {
        return CheckAlignment(true);
    }

    private bool CheckVertical()
    {
        return CheckAlignment(false);
    }

    private bool CheckAlignment(bool isHorizontal)
    {
        foreach (var gridObject in gridSystem.gridObjects)
        {
            Cell otherCell = gridObject.GetComponent<Cell>();
            if (otherCell == null) continue;

            if ((isHorizontal && GetCellPosition(otherCell).x == GetCellPosition(cell).x) ||
                (!isHorizontal && GetCellPosition(otherCell).y == GetCellPosition(cell).y))
            {
                if (!otherCell.allDirActivated)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private Vector2Int GetCellPosition(Cell cell)
    {
        return cell.GetCellPosition();
    }
}
