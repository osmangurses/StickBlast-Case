using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPlacement : MonoBehaviour
{
    public static VirtualPlacement instance;
    [SerializeField] GameObject cellPrefab;
    private Cell cell;
    private GameObject cellObject;
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
        Color activeColor = cell.activeStickColor;
        activeColor.a = 0.7f;
        cell.activeStickColor = activeColor;
        cell._center.gameObject.SetActive(false);
        ReplaceVirtualCell();
    }
    public void PlaceVirtualCell(VirtualCell virtualCell)
    {
        cellObject.transform.localScale = Vector3.one;
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
    }
}
public class VirtualCell
{
    public Vector3 position;
    public bool left, right, up, down;
}
