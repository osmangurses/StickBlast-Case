using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Corner : MonoBehaviour
{
    public bool isActive;
    public Color activeCornerColor;
    public Color inactiveCornerColor = Color.white;
    SpriteRenderer _spriteRenderer;
    [SerializeField]GameObject tempCorner;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void SetCronerActive()
    {
        if (!isActive && gameObject.activeSelf)
        {
            tempCorner.transform.DOScale(transform.localScale, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>_spriteRenderer.color = activeCornerColor);
            isActive = true;
        }
    }
    public void SetCronerInActive()
    {
        if (isActive && gameObject.activeSelf)
        {
            tempCorner.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutBounce).OnComplete(() => _spriteRenderer.color = inactiveCornerColor);
            isActive = false;
        }
    }
}
