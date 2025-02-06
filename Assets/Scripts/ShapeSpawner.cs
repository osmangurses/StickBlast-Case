using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShapeSpawner : MonoBehaviour
{
    public static ShapeSpawner instance;

    public bool isRandom = true;
    public List<GameObject> allShapes;
    public ShapeListContainer shapeListContainer;
    [HideInInspector] public List<GameObject> spawnedShapes = new List<GameObject>();
    [HideInInspector] public ShapeList shapeList;

    [SerializeField] Transform[] positions;
    private int order = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        shapeListContainer = Resources.Load<ShapeListContainer>("ShapeListContainer");
        if (!isRandom)
        {
            if (PlayerPrefs.GetInt("Level", 0) == shapeListContainer.shapeLists.Count)
            {
                PlayerPrefs.SetInt("Level", 0);
            }
            shapeList = shapeListContainer.shapeLists[PlayerPrefs.GetInt("Level", 0)];
        }
        Spawn(3);
    }

    public void Spawn(int count)
    {
        if (!(order >= shapeList.shapes.Count) || isRandom)
        {
            spawnedShapes.Clear();

            for (int i = 0; i < count; i++)
            {
                GameObject selectedShape;

                if (isRandom)
                {
                    selectedShape = allShapes[Random.Range(0, allShapes.Count)];
                    order++;
                }
                else
                {
                    selectedShape = shapeList.shapes[order];
                    order++;
                }

                var spawnedShape = Instantiate(selectedShape, positions[i]);
                spawnedShape.transform.localScale = Vector3.zero;
                spawnedShape.transform.DOScale(Vector3.one,0.3f);
                spawnedShapes.Add(spawnedShape);
            }
            if (order%12==0 && order!=0)
            {
                Cell selectedCell = TouchController.instance.gridSystem.gridObjects[
                    Random.Range(0, TouchController.instance.gridSystem.gridObjects.GetLength(0))
                    , Random.Range(0, TouchController.instance.gridSystem.gridObjects.GetLength(1))]
                    .GetComponent<Cell>();
                selectedCell.AddSkill();
            }
        }
        else
        {
            spawnedShapes.Clear();
        }
    }
    public void MixSpawned()
    {
        int currentShapeCount = 0;
        foreach (var spawnedShape in spawnedShapes)
        {
            if (spawnedShape!=null)
            {
                currentShapeCount++;
                spawnedShape.transform.DOScale(Vector3.zero,0.1f).OnComplete(()=>Destroy(spawnedShape));
            }
        }
        bool tempIsRandom = isRandom;
        spawnedShapes.Clear();
        isRandom = true;
        Spawn(currentShapeCount);
        isRandom = tempIsRandom;
        order -= currentShapeCount;
    }
}

