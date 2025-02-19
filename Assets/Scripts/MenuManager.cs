using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI level_tmp;
    [SerializeField] TMP_InputField gridSizeX, gridSizeY;
    private void Awake()
    {
        level_tmp.text = "Level " + (PlayerPrefs.GetInt("Level", 0) + 1).ToString();
        gridSizeX.text = PlayerPrefs.GetInt("GridSizeX", 4).ToString();
        gridSizeY.text = PlayerPrefs.GetInt("GridSizeY", 4).ToString();
        gridSizeX.onEndEdit.AddListener(value => SetGridSize("GridSizeX", value));
        gridSizeY.onEndEdit.AddListener(value => SetGridSize("GridSizeY", value));

    }
    public void SetGridSize(string prefString,string size)
    {
        int parsedSize = 4;
        if (int.TryParse(size, out parsedSize))
        {
            PlayerPrefs.SetInt(prefString, parsedSize);
        }
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