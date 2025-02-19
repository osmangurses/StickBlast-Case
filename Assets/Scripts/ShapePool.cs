using System.Collections.Generic;
using UnityEngine;

public class ShapePool : MonoBehaviour
{
    public static ShapePool instance;
    [SerializeField] private List<GameObject> shapePrefabs;
    [SerializeField] List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public GameObject Instantiate(string objectName, Transform parent)
    {
        foreach (var obj in pool)
        {
            if (obj.name.StartsWith(objectName) && !obj.activeSelf)
            {
                obj.transform.parent = parent;
                obj.transform.localPosition = Vector3.zero;
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject prefab = shapePrefabs.Find(obj => obj.name == objectName);
        if (prefab == null)
        {
            Debug.LogWarning($"Prefab bulunamadý: {objectName}");
            return null;
        }

        var tempObj = Instantiate(prefab, parent);
        tempObj.name = objectName;
        tempObj.transform.localPosition = Vector3.zero;
        pool.Add(tempObj);
        return tempObj;
    }

    public void Destroy(GameObject destroyingObject)
    {
        if (pool.Contains(destroyingObject))
        {
            destroyingObject.SetActive(false);
            destroyingObject.transform.parent = null;
        }
    }
}
