using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPlacement : MonoBehaviour
{
    public static VirtualPlacement instance;
    [SerializeField] GameObject cellPrefab;
    private Cell cell;
    public GameObject cellObject;
    private Vector3 baseScale;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cellObject = Instantiate(cellPrefab);
        cell = cellObject.GetComponent<Cell>(); 
        if (cellObject != null)
        {
            foreach (SpriteRenderer sr in cellObject.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.sortingOrder += 1;
            }
        }
        cellObject.GetComponent<BoxCollider2D>().enabled = false;
        baseScale = TouchController.instance.gridSystem.cellSize.x *Vector3.one;
        Color activeColor = cell.activeStickColor;
        activeColor.a = 0.7f;
        cell.activeStickColor = activeColor;
        cell._center.gameObject.SetActive(false);
        //baseScale = cell.transform.localScale;
        ReplaceVirtualCell();
    }
    public void PlaceVirtualCell(VirtualCell virtualCell)
    {
        cell.up = cell.down = cell.left = cell.right = false;
        cellObject.transform.localScale = baseScale;
        cellObject.transform.position = virtualCell.position;
        cell.DeactivateDir(Direction.Up);
        cell.DeactivateDir(Direction.Down);
        cell.DeactivateDir(Direction.Left);
        cell.DeactivateDir(Direction.Right);
        cell.ActivateDirections(virtualCell.up, virtualCell.down, virtualCell.left, virtualCell.right);
    }
    public void ReplaceVirtualCell()
    {
        cellObject.transform.localScale = Vector3.zero;
        cell.up = cell.down = cell.left = cell.right = false;
        cellObject.transform.position = Vector3.one * 999;
    }
}
public class VirtualCell
{
    public Vector3 position;
    public bool left, right, up, down;
}
