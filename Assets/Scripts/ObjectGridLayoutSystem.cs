using UnityEngine;

public class ObjectGridLayoutSystem : MonoBehaviour
{
    public enum Corner { UpperLeft, UpperRight, LowerLeft, LowerRight, Middle }
    public enum Axis { Horizontal, Vertical }
    public enum Constraint { Flexible, FixedColumnCount, FixedRowCount }
    public enum ChildAlignment
    {
        UpperLeft, UpperCenter, UpperRight,
        MiddleLeft, MiddleCenter, MiddleRight,
        LowerLeft, LowerCenter, LowerRight
    }

    [Header("Grid Settings")]
    public GameObject prefab;
    public int columns = 4;
    public int rows = 4;
    public Vector2 cellSize = new Vector2(1, 1);
    public Vector2 spacing = new Vector2(0.2f, 0.2f);

    [Header("Layout Settings")]
    public Corner startCorner = Corner.UpperLeft;
    public Axis startAxis = Axis.Horizontal;
    public ChildAlignment childAlignment = ChildAlignment.MiddleCenter;
    public Constraint constraint = Constraint.Flexible;
    public int constraintCount = 2;

    [Header("Generated Grid (Read-Only)")]
    public GameObject[,] gridObjects;
    private void Awake()
    {
        columns = PlayerPrefs.GetInt("GridSizeX");
        rows = PlayerPrefs.GetInt("GridSizeY");
        float sizeMultiplier = (4f / Mathf.Max(rows, columns));
        Debug.Log($"Size Multiplier = {sizeMultiplier}");
        transform.localScale = Vector3.one * sizeMultiplier;
        cellSize = Vector2.one * sizeMultiplier * 0.8333333f;
    }
    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab is not assigned.");
            return;
        }

        ClearGrid();

        int countX = columns;
        int countY = rows;

        if (constraint == Constraint.FixedColumnCount)
        {
            countX = constraintCount;
            countY = Mathf.CeilToInt((float)(rows * columns) / constraintCount);
        }
        else if (constraint == Constraint.FixedRowCount)
        {
            countY = constraintCount;
            countX = Mathf.CeilToInt((float)(rows * columns) / constraintCount);
        }

        gridObjects = new GameObject[countX, countY];

        for (int y = 0; y < countY; y++)
        {
            for (int x = 0; x < countX; x++)
            {
                Vector2 position = CalculatePosition(x, y, countX, countY);
                GameObject obj = Instantiate(prefab, (Vector2)transform.position + position, Quaternion.identity, transform);
                AlignChild(obj.transform, x, y, countX, countY);

                gridObjects[x, y] = obj;
            }
        }
    }

    private Vector2 CalculatePosition(int x, int y, int totalColumns, int totalRows)
    {
        float posX = (cellSize.x + spacing.x) * x;
        float posY = (cellSize.y + spacing.y) * y;

        switch (startCorner)
        {
            case Corner.UpperLeft: return new Vector2(posX, -posY);
            case Corner.UpperRight: return new Vector2(-posX, -posY);
            case Corner.LowerLeft: return new Vector2(posX, posY);
            case Corner.LowerRight: return new Vector2(-posX, posY);
            case Corner.Middle: return CalculateMiddlePosition(x, y, totalColumns, totalRows);
            default: return Vector2.zero;
        }
    }

    private Vector2 CalculateMiddlePosition(int x, int y, int totalColumns, int totalRows)
    {
        float totalWidth = (cellSize.x + spacing.x) * totalColumns - spacing.x;
        float totalHeight = (cellSize.y + spacing.y) * totalRows - spacing.y;

        float startX = -totalWidth / 2 + (cellSize.x / 2);
        float startY = totalHeight / 2 - (cellSize.y / 2);

        float posX = startX + x * (cellSize.x + spacing.x);
        float posY = startY - y * (cellSize.y + spacing.y);

        return new Vector2(posX, posY);
    }

    private void AlignChild(Transform child, int x, int y, int totalColumns, int totalRows)
    {
        Vector2 offset = Vector2.zero;

        float totalWidth = (cellSize.x + spacing.x) * totalColumns - spacing.x;
        float totalHeight = (cellSize.y + spacing.y) * totalRows - spacing.y;

        switch (childAlignment)
        {
            case ChildAlignment.UpperLeft: offset = new Vector2(0, 0); break;
            case ChildAlignment.UpperCenter: offset = new Vector2((totalWidth - cellSize.x) / 2f, 0); break;
            case ChildAlignment.UpperRight: offset = new Vector2(totalWidth - cellSize.x, 0); break;
            case ChildAlignment.MiddleLeft: offset = new Vector2(0, -(totalHeight - cellSize.y) / 2f); break;
            case ChildAlignment.MiddleCenter: offset = new Vector2((totalWidth - cellSize.x) / 2f, -(totalHeight - cellSize.y) / 2f); break;
            case ChildAlignment.MiddleRight: offset = new Vector2(totalWidth - cellSize.x, -(totalHeight - cellSize.y) / 2f); break;
            case ChildAlignment.LowerLeft: offset = new Vector2(0, -totalHeight + cellSize.y); break;
            case ChildAlignment.LowerCenter: offset = new Vector2((totalWidth - cellSize.x) / 2f, -totalHeight + cellSize.y); break;
            case ChildAlignment.LowerRight: offset = new Vector2(totalWidth - cellSize.x, -totalHeight + cellSize.y); break;
        }

        child.localPosition += (Vector3)offset;
    }

    public void ClearGrid()
    {
        if (gridObjects != null)
        {
            foreach (var obj in gridObjects)
            {
                if (obj != null)
                    DestroyImmediate(obj);
            }
        }

        gridObjects = null;
    }

    public GameObject[,] GetGridObjects()
    {
        return gridObjects;
    }
}
