using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{
    public static TouchController instance;

    public LayerMask targetLayer;
    public LayerMask cellLayer;
    public ObjectGridLayoutSystem gridSystem;
    public Vector3 offset;

    private Camera mainCamera;
    private int shapeCount = 0;

    private TouchInputHandler inputHandler;
    private ShapeDirectionHandler directionHandler;
    private CellValidator cellValidator;

    private void Awake()
    {
        instance = this;
        inputHandler = new TouchInputHandler(this);
        directionHandler = new ShapeDirectionHandler();
        cellValidator = new CellValidator(gridSystem);
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        inputHandler.HandleMouseInput();
    }

    public bool OnShapeDropped(GameObject obj, Cell targetCell)
    {
        Shape shape = obj.GetComponent<Shape>();
        directionHandler.AdjustShapeDirection(shape, targetCell);

        if (cellValidator.CheckCellDirs(shape, targetCell))
        {
            Vector2Int cellPosition = cellValidator.GetCellPosition(targetCell);
            shape.OnDropOnCell(targetCell, gridSystem, cellPosition);
            shapeCount++;

            if (shapeCount == 3)
            {
                ShapeSpawner.instance.Spawn(3);
                shapeCount = 0;
            }
            obj.transform.DOScale(Vector3.zero,0.1f);
            Destroy(obj,0.3f);
            StartCoroutine(CheckAndReload());
            ActionEvents.InvokeShapePlaced();
            return true;
        }
        else
        {
            obj.GetComponent<Collider2D>().enabled = true;
            return false;
        }
    }
    public VirtualCell CanShapeDrop(GameObject obj, Cell targetCell)
    {
        VirtualCell tempCell= new VirtualCell();
        Shape shape = obj.GetComponent<Shape>();
        directionHandler.AdjustShapeDirection(shape, targetCell);

        if (cellValidator.CheckCellDirs(shape, targetCell))
        {
            tempCell.position = targetCell.transform.position;
            if (shape.shapeInfo.centerCellDirections.Contains(Direction.Up) || targetCell.up)
            {
                tempCell.up = true;
            }
            if (shape.shapeInfo.centerCellDirections.Contains(Direction.Down) || targetCell.down)
            {
                tempCell.down = true;
            }
            if (shape.shapeInfo.centerCellDirections.Contains(Direction.Left) || targetCell.left)
            {
                tempCell.left = true;
            }
            if (shape.shapeInfo.centerCellDirections.Contains(Direction.Right) || targetCell.right)
            {
                tempCell.right = true;
            }
        }
        else
        {
            tempCell = null;
        }
        return tempCell;
    }

    IEnumerator CheckAndReload()
    {
        yield return new WaitForSeconds(1f);
        if (!cellValidator.CanPlaceAnyShape())
        {
            ActionEvents.InvokeNoAvailableSpace();
        }
    }
}
