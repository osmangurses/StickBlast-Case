using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public SpriteRenderer skillVisual;
    public Color activeStickColor;
    public Color inactiveStickColor = Color.white;
    public bool up, down, right, left;
    public bool allDirActivated;
    public SpriteRenderer _upStick, _downStick, _leftStick, _rightStick, _center;
    public Corner _upperLeftCorner, _upperRightCorner, _lowerRightCorner, _lowerLeftCorner;

    private ObjectGridLayoutSystem gridSystem;
    private CellDirectionHandler directionHandler;
    private CellEffectHandler effectHandler;
    private CellBlastHandler blastHandler;
    private SkillType skill = SkillType.Null;
    private void Start()
    {
        gridSystem = TouchController.instance.gridSystem;
        directionHandler = new CellDirectionHandler(this);

        effectHandler = new CellEffectHandler(this, _upStick, _downStick, _leftStick, _rightStick, _center,
            _upperLeftCorner, _upperRightCorner, _lowerRightCorner, _lowerLeftCorner);

        blastHandler = new CellBlastHandler(this, gridSystem);
    }
    public Cell GetNeighborCell(Vector2Int offset)
    {
        Vector2Int cellPos = GetCellPosition();
        var targetCellPos = cellPos + offset;
        if (targetCellPos.x<0|| targetCellPos.x>=gridSystem.rows || targetCellPos.y < 0 || targetCellPos.y >= gridSystem.columns) { return null; }
        return gridSystem.gridObjects[targetCellPos.x, targetCellPos.y].GetComponent<Cell>();

    }
    public void ActivateDirections(bool up, bool down, bool left, bool right)
    {
        directionHandler.ActivateDirections(up, down, left, right);
        effectHandler.UpdateEffects();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (GetNeighborCell(new Vector2Int(i, j)) != null)
                {
                    GetNeighborCell(new Vector2Int(i, j)).CheckCorner();
                }
            }
        }
        if (directionHandler.IsAllDirActivated())
        {
            effectHandler.ActivateCenterEffect();
            allDirActivated = true;
            StartCoroutine(blastHandler.Blast());
        }
    }

    public void DeactivateDir(Direction dir)
    {
        if (!allDirActivated)
        {
            directionHandler.DeactivateDir(dir);
            effectHandler.UpdateEffects();
            effectHandler.ResetCenterEffect();
            allDirActivated = false;
        }
    }
    public void AddSkill()
    {
        SkillType[] skillValues = (SkillType[])System.Enum.GetValues(typeof(SkillType));
        SkillType randomSkill = skillValues[Random.Range(1, skillValues.Length)];
        while (randomSkill==SkillType.Null)
        {
            randomSkill = skillValues[Random.Range(1, skillValues.Length)];
        }
        skill = randomSkill;
        skillVisual.sprite = SkillManager.instance.GetSpriteOfSkill(skill);
        skillVisual.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        SkillManager.instance.AddSkill(skill);
        skillVisual.gameObject.SetActive(false);
        directionHandler.DeactivateAll();
        StartCoroutine(DelayedDeactivation());
    }

    private IEnumerator DelayedDeactivation()
    {
        yield return new WaitForSeconds(0.011f);
        effectHandler.ResetEffects();
        blastHandler.HandleDeactivation();
    }

    public void CheckCorner()
    {
        effectHandler.UpdateCorner();
    }
    public Vector2Int GetCellPosition()
    {
        for (int x = 0; x < gridSystem.columns; x++)
        {
            for (int y = 0; y < gridSystem.rows; y++)
            {
                if (gridSystem.gridObjects[x, y] == this.gameObject)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-99, -99);
    }
}
