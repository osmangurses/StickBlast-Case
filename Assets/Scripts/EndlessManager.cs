using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using DG.Tweening;
using TMPro;
public class EndlessManager : MonoBehaviour
{
    [SerializeField] GameObject FailedPanel, FailedPanelBG;
    [SerializeField] TextMeshProUGUI scoreTMP,finalScore_tmp,failed_tmp,bestScore_tmp;
    int placedShapeCount=0;
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
        placedShapeCount++;
        scoreTMP.text = placedShapeCount.ToString();
    }

    private void OnNoAvailableSpace()
    {
        FailedPanel.SetActive(true);
        FailedPanelBG.transform.DOScale(Vector3.one, 0.5f);
        if (placedShapeCount > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore",placedShapeCount);
            failed_tmp.text = "NEW BEST";
        }
        bestScore_tmp.text = "BEST SCORE: " + PlayerPrefs.GetInt("BestScore",0);
        finalScore_tmp.text = placedShapeCount.ToString();
        scoreTMP.text = "";
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoHome()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
