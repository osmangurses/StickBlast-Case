using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class CellEffectHandler
{
    private Cell cell;
    private SpriteRenderer upStick, downStick, leftStick, rightStick, center;
    private Corner upperLeftCorner, upperRightCorner, lowerRightCorner, lowerLeftCorner;

    public CellEffectHandler(Cell cell, SpriteRenderer upStick, SpriteRenderer downStick, SpriteRenderer leftStick, SpriteRenderer rightStick, SpriteRenderer center, Corner upperLeftCorner, Corner upperRightCorner, Corner lowerRightCorner, Corner lowerLeftCorner)
    {
        this.cell = cell;
        this.upStick = upStick;
        this.downStick = downStick;
        this.leftStick = leftStick;
        this.rightStick = rightStick;
        this.center = center;
        this.upperLeftCorner = upperLeftCorner;
        this.upperRightCorner = upperRightCorner;
        this.lowerRightCorner = lowerRightCorner;
        this.lowerLeftCorner = lowerLeftCorner;
    }

    public void UpdateEffects()
    {
        UpdateStick(upStick, cell.up);
        UpdateStick(downStick, cell.down);
        UpdateStick(leftStick, cell.left);
        UpdateStick(rightStick, cell.right);
        UpdateCorner();
    }

    public void ActivateCenterEffect()
    {
        center.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBounce);
    }

    public void ResetCenterEffect()
    {
        center.transform.DOScale(Vector3.zero, 0f).SetEase(Ease.InBounce);
    }

    public void ResetEffects()
    {
        UpdateEffects();
        UpdateCorner();
        ResetCenterEffect();
    }

    private void UpdateStick(SpriteRenderer stick, bool isActive)
    {
        if (stick != null)
        {
            stick.color = isActive ? cell.activeStickColor : cell.inactiveStickColor;
        }
    }

    public void UpdateCorner()
    {
        if (cell.up || cell.left)
        {
            upperLeftCorner.SetCronerActive();
        }
        else
        {
            upperLeftCorner.SetCronerInActive();
        }

        if (cell.up || cell.right)
        {
            upperRightCorner.SetCronerActive();
        }
        else
        {
            upperRightCorner.SetCronerInActive();
        }

        if (cell.down || cell.left)
        {
            lowerLeftCorner.SetCronerActive();
        }
        else
        {
            lowerLeftCorner.SetCronerInActive();
        }

        if (cell.down || cell.right)
        {
            lowerRightCorner.SetCronerActive();
        }
        else
        {
            lowerRightCorner.SetCronerInActive();
        }

        RunDelayedCheckNeighborCorner();
    }

    private async void RunDelayedCheckNeighborCorner()
    {
        await Task.Delay(10);
        CheckNeighborCorner();
    }

    private void CheckNeighborCorner()
    {
        Vector2Int[] neighborOffsets =
        {
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1),
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 0), new Vector2Int(-1, 0)
        };

        foreach (var offset in neighborOffsets)
        {
            var neighbor = cell.GetNeighborCell(offset);
            if (neighbor == null) continue;

            if (offset == new Vector2Int(1, 1) && neighbor._upperLeftCorner.isActive) lowerRightCorner.SetCronerActive();
            if (offset == new Vector2Int(-1, 1) && neighbor._upperRightCorner.isActive) lowerLeftCorner.SetCronerActive();
            if (offset == new Vector2Int(1, -1) && neighbor._lowerLeftCorner.isActive) upperRightCorner.SetCronerActive();
            if (offset == new Vector2Int(-1, -1) && neighbor._lowerRightCorner.isActive) upperLeftCorner.SetCronerActive();

            if (offset == new Vector2Int(0, 1))
            {
                if (neighbor._upperRightCorner.isActive) lowerRightCorner.SetCronerActive();
                if (neighbor._upperLeftCorner.isActive) lowerLeftCorner.SetCronerActive();
            }
            if (offset == new Vector2Int(0, -1))
            {
                if (neighbor._lowerRightCorner.isActive) upperRightCorner.SetCronerActive();
                if (neighbor._lowerLeftCorner.isActive) upperLeftCorner.SetCronerActive();
            }
            if (offset == new Vector2Int(1, 0))
            {
                if (neighbor._upperLeftCorner.isActive) upperRightCorner.SetCronerActive();
                if (neighbor._lowerLeftCorner.isActive) lowerRightCorner.SetCronerActive();
            }
            if (offset == new Vector2Int(-1, 0))
            {
                if (neighbor._upperRightCorner.isActive) upperLeftCorner.SetCronerActive();
                if (neighbor._lowerRightCorner.isActive) lowerLeftCorner.SetCronerActive();
            }
        }
    }
}
