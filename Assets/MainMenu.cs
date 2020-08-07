using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup fader;

    private void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alphaCanvas(fader, 0, 2);
    }

    public void StartGame()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;
        
        LeanTween.alphaCanvas(fader, 1, 2).setOnComplete(()=>{
            ao.allowSceneActivation = true;            
        });
    }
}
