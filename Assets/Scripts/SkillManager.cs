using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Button bomb_btn, mix_btn, sword_btn;
    public int bombAmount, mixAmount, swordAmount = 1;
    public SkillType selectedSkill=SkillType.Null;
    public GameObject skillObject;
    public Sprite bombSprite, mixSprite, swordSprite;
    public TextMeshProUGUI bombAmount_tmp, mixAmount_tmp, swordAmount_tmp;

    private SpriteRenderer skillObjectSR;

    private ObjectGridLayoutSystem gridSystem;
    private void Awake()
    {
        instance = this;
        skillObjectSR = skillObject.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        gridSystem = TouchController.instance.gridSystem; 
        bomb_btn.onClick.AddListener(() => SelectSkill(SkillType.Bomb));
        mix_btn.onClick.AddListener(() => SelectSkill(SkillType.Mix));
        sword_btn.onClick.AddListener(() => SelectSkill(SkillType.Sword));
        UpdateAmountOnUI();

    }
    public void SelectSkill(SkillType type)
    {
        if ((type==SkillType.Bomb && bombAmount<1) ||(type==SkillType.Mix && mixAmount<1) ||(type==SkillType.Sword && swordAmount<1))
        {
            return;
        }
        selectedSkill = type;
        if (selectedSkill==SkillType.Mix)
        {
            UseSkill(null);
        }
    }
    public void DeselectSkill()
    {
        selectedSkill = SkillType.Null;
    }
    public void UseSkill(Cell targetCell)
    {
        switch (selectedSkill)
        {
            case SkillType.Null:
                return;
            case SkillType.Bomb:
                UseBomb(targetCell);
                break;
            case SkillType.Sword:
                UseSword(targetCell);
                break;
            case SkillType.Mix:
                UseMix();
                break;
        }
        selectedSkill = SkillType.Null;
        UpdateAmountOnUI();
    }

    public void AddSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.Bomb:
                bombAmount++;
                break;
            case SkillType.Sword:
                swordAmount++;
                break;
            case SkillType.Mix:
                mixAmount++;
                break;
        }
        UpdateAmountOnUI();
    }

    public Sprite GetSpriteOfSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.Bomb:
                return bombSprite;
            case SkillType.Sword:
                return swordSprite;
            default:
                return mixSprite;
        }
    }

    void UpdateAmountOnUI()
    {
        bombAmount_tmp.text = "x" + bombAmount.ToString();
        mixAmount_tmp.text = "x" + mixAmount.ToString();
        swordAmount_tmp.text = "x" + swordAmount.ToString();
    }
    void UseSword(Cell cell)
    {
        skillObjectSR.sprite = swordSprite;
        skillObject.transform.position = new Vector3(cell.gameObject.transform.position.x-0.6f, 3, 0);
        skillObject.transform.DOScale(Vector3.one*2,0.2f);
        skillObject.transform.DOMoveY(-3, 1f).OnComplete(()=> skillObject.transform.DOScale(Vector3.zero,0.3f));

        foreach (var obj in gridSystem.gridObjects)
        {
            if (obj.GetComponent<Cell>().GetCellPosition().x==cell.GetCellPosition().x)
            {
                obj.GetComponent<Cell>().Deactivate();
            }
        }
        swordAmount--;
    }
    void UseBomb(Cell cell)
    {
        skillObjectSR.sprite = bombSprite;
        skillObject.transform.localScale = 30 * Vector3.one;
        skillObject.transform.position = cell.gameObject.transform.position;
        skillObject.transform.DOScale(Vector3.one * 0.5f, 1).OnComplete(()=>skillObject.transform.localScale=Vector3.zero);
        foreach (var obj in gridSystem.gridObjects)
        {
            Cell objCell = obj.GetComponent<Cell>();
            if ((Mathf.Abs(cell.GetCellPosition().y-objCell.GetCellPosition().y)< 2 && cell.GetCellPosition().x == objCell.GetCellPosition().x) || (Mathf.Abs(cell.GetCellPosition().x - objCell.GetCellPosition().x)<2 && cell.GetCellPosition().y == objCell.GetCellPosition().y))
            {
                obj.GetComponent<Cell>().Deactivate();
            }
        }
        bombAmount--;
    }
    void UseMix()
    {
        skillObjectSR.sprite = mixSprite;
        skillObject.transform.position = new Vector3(0, -5, 0);
        skillObject.transform.DOScale(Vector3.one*6, 0.01f);
        skillObject.transform.DOShakeRotation(1f).OnComplete(() => skillObject.transform.DOScale(Vector3.zero, 0.3f));
        ShapeSpawner.instance.MixSpawned();
        mixAmount--;
    }
}
public enum SkillType
{
    Null,
    Bomb,
    Mix,
    Sword
}
