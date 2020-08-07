using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup fader;

    public Canvas main;
    public Canvas settings;

    public TMP_Dropdown resolution;
    public Toggle fullscreenToggle;

    public TMP_Dropdown quality;
    public TextMeshProUGUI newGameButton;

    public Material wormhole;

    private void Start()
    {
        ExitSettings();
        fader.gameObject.SetActive(true);
        LeanTween.alphaCanvas(fader, 0, 3).setDelay(2);
        EpilepsyMode(false);

        resolution.ClearOptions();
        List<string> resList = new List<string>();
        for(int i = Screen.resolutions.Length - 1; i >0; i--)
        {
            Resolution r = Screen.resolutions[i];
            resList.Add(r.width + " x " + r.height + " : " + r.refreshRate);
        }
        resolution.AddOptions(resList);

        quality.ClearOptions();
        List<string> qualityList = new List<string>(QualitySettings.names);
        quality.AddOptions(qualityList);

        quality.value = QualitySettings.GetQualityLevel();

        RefreshButton();
    }

    public void RefreshButton()
    {
        GlobalGameSettings.LoadProgress();
        if (GlobalGameSettings.lastLevel == 1)
        {
            newGameButton.text = "New Game";
        }
        else
            newGameButton.text = "Continue";
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/save"))
            File.Delete(Application.persistentDataPath + "/save");

        RefreshButton();
    }

    public void StartGame()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(GlobalGameSettings.lastLevel);
        ao.allowSceneActivation = false;
        
        LeanTween.alphaCanvas(fader, 1, 2).setOnComplete(()=>{
            ao.allowSceneActivation = true;            
        });
    }

    public void EnterSettings()
    {
        main.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }

    public void ExitSettings()
    {
        main.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void ChangeFullscreen()
    {
        SetResolution(resolution.value);
    }

    public void SetResolution(int option)
    {
        Resolution r = Screen.resolutions[Screen.resolutions.Length - 1 - option];
        Screen.SetResolution(r.width, r.height, fullscreenToggle.isOn, r.refreshRate);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void EpilepsyMode(bool t)
    {
        wormhole.SetFloat("_DesaturationAmount", t ? 0 : 1);
    }
}
