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
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCronerActive()
    {
        if (!isActive && gameObject.activeSelf)
        {
            _spriteRenderer.color = activeCornerColor;
            isActive = true;
        }
    }
    public void SetCronerInActive()
    {
        if (isActive && gameObject.activeSelf)
        {
            _spriteRenderer.color = inactiveCornerColor;
            isActive = false;
        }
    }
}
