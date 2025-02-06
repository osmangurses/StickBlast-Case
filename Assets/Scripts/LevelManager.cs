using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject levelFailedPanel, levelCompletePanel, levelFailedPanelBG, levelCompletePanelBG;
    int levelShapeCount;
    private void Start()
    {
        levelShapeCount = ShapeSpawner.instance.shapeList.shapes.Count;
    }
    private void OnEnable()
    {
        ActionEvents.ShapePlaced += OnShapePlaced;
        ActionEvents.NoAvailableSpace += OnNoAvailableSpace;
    }

    private void OnDisable()
    {
        ActionEvents.ShapePlaced -= OnShapePlaced;
        ActionEvents.NoAvailableSpace -= OnNoAvailableSpace;
    }

    private void OnShapePlaced()
    {
        levelShapeCount--;
        if (levelShapeCount==0)
        {
            levelCompletePanel.SetActive(true);
            levelCompletePanelBG.transform.DOScale(Vector3.one,0.5f);
            PlayerPrefs.SetInt("Level",PlayerPrefs.GetInt("Level",0)+1);
        }
    }

    private void OnNoAvailableSpace()
    {
        levelFailedPanel.SetActive(true);
        levelFailedPanelBG.transform.DOScale(Vector3.one, 0.5f);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoHome()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void NextLevel()
    {
        RestartLevel();
    }

}
