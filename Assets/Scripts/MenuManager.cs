using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI level_tmp;
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        level_tmp.text = "Level " + (PlayerPrefs.GetInt("Level", 0) + 1).ToString();
    }
    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }
    public void LevelBasedScene()
    {
        SceneManager.LoadScene("LevelBased");
    }
    public void EndlessScene()
    {
        SceneManager.LoadScene("Endless");
    }
}