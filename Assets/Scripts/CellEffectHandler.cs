using UnityEngine;
using DG.Tweening;

public class CellEffectHandler
{
    private Cell cell;
    private SpriteRenderer upStick, downStick, leftStick, rightStick, center;

    public CellEffectHandler(Cell cell, SpriteRenderer upStick, SpriteRenderer downStick, SpriteRenderer leftStick, SpriteRenderer rightStick, SpriteRenderer center)
    {
        this.cell = cell;
        this.upStick = upStick;
        this.downStick = downStick;
        this.leftStick = leftStick;
        this.rightStick = rightStick;
        this.center = center;
    }

    public void UpdateEffects()
    {
        UpdateStick(upStick, cell.up);
        UpdateStick(downStick, cell.down);
        UpdateStick(leftStick, cell.left);
        UpdateStick(rightStick, cell.right);
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
        upStick.color = downStick.color = leftStick.color = rightStick.color = cell.inactiveStickColor;
        ResetCenterEffect();
    }

    private void UpdateStick(SpriteRenderer stick, bool isActive)
    {
        if (stick != null)
        {
            stick.color = isActive ? cell.activeStickColor : cell.inactiveStickColor;
        }
    }
}
