using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TMP_Dropdown resolotionDropDown, qualityDropDown;
    Resolution[] resolutions;

    void Start()
    {
        qualityDropDown.value = QualitySettings.GetQualityLevel();
        CalculateResolution();
    }
    void Update()
    {
        BackToMenu();
        Debug.Log(QualitySettings.GetQualityLevel());
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        if(settings.activeInHierarchy)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Back();
            }
        }
    }

    public void Back()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SetResolution(int currentResIndex)
    {
        Resolution resolution = resolutions[currentResIndex];
        Screen.SetResolution(resolution.width, resolution.width, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen) 
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    private void CalculateResolution()
    {
        resolutions = Screen.resolutions;

        resolotionDropDown.ClearOptions();
        
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].height == Screen.currentResolution.height &&
                 resolutions[i].width== Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }
        resolotionDropDown.AddOptions(options);
        resolotionDropDown.value = currentResolutionIndex;
        resolotionDropDown.RefreshShownValue();
    }
}
