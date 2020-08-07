using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;

public class LevelInfo : MonoBehaviour
{

    #region Singleton
    public static LevelInfo instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    #endregion

    public CanvasGroup fader;

    public string levelName;
    [Multiline]
    public string levelDescription;

    public bool eliminateEnemies;
    public bool time;
    public bool portal = true;

    public float playerHealth;
    public float playerEnergy;

    public int enemyCount;
    public float timeToComplete;
    public int nextLevel;

    [System.NonSerialized]
    public bool reachedPortal = false;
    [System.NonSerialized]
    public int killedEnemies = 0;
    [System.NonSerialized]
    public float timePassed = 0f;

    public CanvasGroup panel, goalKills, goalTime, goalPortal;
    public TextMeshProUGUI timelimit, killlimit, levelname, leveldescription, health, energy;
    public GameObject crosshair;
    public GameObject goalInfo;
    public float typeWriterSpeed = 0.1f;


    public VideoPlayer maskPlayer, videoPlayer;

    public GameObject display;
    public CanvasGroup portalGoalCheck, killGoalCheck, timeGoalCheck;
    public TextMeshProUGUI portalGoalCheckText, killGoalCheckText, timeGoalCheckText;
    public GameObject levelFailed;
    public CanvasGroup hint;
    public TextMeshProUGUI hintText;
    private GameObject levelCamera;

    private bool timerRunning = false;
    private bool startingSequence = false;

    

    private void Start()
    {
        levelFailed.SetActive(false);

        maskPlayer.time = 3;
        videoPlayer.time = 3;

        videoPlayer.Play();
        maskPlayer.Play();
        display.SetActive(true);

        fader.alpha = 1;
        crosshair.SetActive(false);
        goalInfo.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        levelCamera = GameObject.FindGameObjectWithTag("LevelCamera");
        LeanTween.alphaCanvas(fader, 0, 1);

        levelname.text = levelName;
        leveldescription.text = "";
        timelimit.text = "Time goal: " + timeToComplete + "s";
        killlimit.text = "Kill goal: " + enemyCount + " enemies";
        LeanTween.delayedCall(6.5f, () =>
        {
            videoPlayer.Stop();
            maskPlayer.Stop();
            display.SetActive(false);
            LeanTween.alphaCanvas(panel, 1, 2).setOnComplete(() =>
            {
                startingSequence = true;
                StartCoroutine("WriteDescription");
                LeanTween.delayedCall(levelDescription.Length * typeWriterSpeed + 1.3f, () =>
                {
                    StartCoroutine("CountEnergy");
                    LeanTween.delayedCall(2f, () =>
                    {
                        StartCoroutine("CountHealth");
                        LeanTween.delayedCall(2f, () =>
                        {
                            Goals();
                        });
                    });
                });
            });
        });
    }

    private string TypeWrite(string t, int c)
    {
        if (t.Length < c)
            return "";
        string output = "";
        for(int i = 0; i<c; i++)
        {
            output = output + t[i];
        }
        return output;
    }

    private IEnumerator WriteDescription()
    {

        for(int i = 0; i < levelDescription.Length; i++)
        {
            leveldescription.text = TypeWrite(levelDescription, i);
            yield return new WaitForSeconds(typeWriterSpeed);
        }
    }

    private IEnumerator CountHealth()
    {

        for (int i = 0; i <= playerHealth; i++)
        {
            health.text = i.ToString();
            yield return new WaitForSeconds(1f / playerHealth);
        }
    }

    private IEnumerator CountEnergy()
    {

        for (int i = 0; i <= playerEnergy; i++)
        {
            energy.text = i.ToString();
            yield return new WaitForSeconds(1f / playerEnergy);
        }
    }

    private void Goals()
    {
        
        if (eliminateEnemies)
            Kills();
        LeanTween.delayedCall(1,()=>{
            if (portal)
                Portal();
            LeanTween.delayedCall(1, () =>
            {
                if (time)
                    Stopwatch();

                LeanTween.delayedCall(3, () =>
                {
                    EndStartSequence();
                });
            });
        });
        
        
    }


    private void EndStartSequence()
    {
        if (!startingSequence) return;
        startingSequence = false;
        LeanTween.alphaCanvas(fader, 1, 1).setOnComplete(() =>
        {
            panel.alpha = 0;
            Player.instance.gameObject.SetActive(true);
            levelCamera.SetActive(false);
            LeanTween.alphaCanvas(fader, 0, 1);
            crosshair.SetActive(true);
            goalInfo.SetActive(true);
            timerRunning = true;
        });
    }
    private void Kills()
    {
        LeanTween.alphaCanvas(goalKills, 1, 1);
    }

    private void Stopwatch()
    {
        LeanTween.alphaCanvas(goalTime, 1, 1);
    }

    private void Portal()
    {
        LeanTween.alphaCanvas(goalPortal, 1, 1);
    }

    private void Update()
    {
        if (startingSequence && Input.GetButton("Jump"))
            EndStartSequence();

        if(timerRunning)
        {
            timePassed += Time.unscaledDeltaTime;
            bool[] checks = new bool[3];
            //Time goal
            if (time)
            {
                checks[0] = timePassed <= timeToComplete;
                timeGoalCheck.alpha = checks[0] ? 1f : 0.2f;
                timeGoalCheckText.text = timePassed.ToString("0.00") + "s / " + timeToComplete + "s";
            }
            else
            {
                checks[0] = true;
                timeGoalCheck.alpha = 0;                
            }
            //portal goal
            if (portal)
            {
                checks[1] = reachedPortal;
                portalGoalCheck.alpha = checks[1] ? 1f : 0.2f;
                portalGoalCheckText.text = checks[1] ? "Reached" : "X";
            }
            else
            {
                checks[1] = true;
                portalGoalCheck.alpha = 0;
            }
            //Elimination goal
            if (eliminateEnemies)
            {
                checks[2] = killedEnemies >= enemyCount;
                killGoalCheck.alpha = checks[2] ? 1f : 0.2f;
                killGoalCheckText.text = killedEnemies + "/" + enemyCount;
            } 
            else
            {
                checks[2] = true;
                killGoalCheck.alpha = 0;
            }

            if (checks[0] && checks[1] && checks[2])
            {
                timerRunning = false;
                LoadNextLevel();               
            }

            if(!checks[0])
            {
                Player.instance.Die();
            }
        }
    }

    public void LoadNextLevel()
    {
        GlobalGameSettings.lastLevel = nextLevel;
        GlobalGameSettings.SaveProgress();
        Time.timeScale = 1;
        LeanTween.delayedCall(2.1f, () =>
        {
            Player.instance.gameObject.SetActive(false);
            levelCamera.SetActive(true);
        });
        videoPlayer.time = 0;
        maskPlayer.time = 0;
        videoPlayer.Play();
        maskPlayer.Play();
        display.SetActive(true);
        AsyncOperation o = SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Single);
        o.allowSceneActivation = false;
        LeanTween.delayedCall(3, () => {
            videoPlayer.Stop();
            maskPlayer.Stop();
            o.allowSceneActivation = true;
        });
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        Debug.Log("Reloading level");
        LeanTween.delayedCall(2.1f, () =>
        {            
            Player.instance.gameObject.SetActive(false);   
            levelCamera.SetActive(true);
        });
        Debug.Log("resetting players");
        videoPlayer.time = 0;
        maskPlayer.time = 0;
        videoPlayer.Play();
        maskPlayer.Play();
        display.SetActive(true);
        AsyncOperation o = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        o.allowSceneActivation = false;
        LeanTween.delayedCall(3, () =>
        {
            videoPlayer.Stop();
            maskPlayer.Stop();
            o.allowSceneActivation = true;
        });
    }


    public void Death()
    {
        levelFailed.SetActive(true);
        LeanTween.alphaCanvas(levelFailed.GetComponent<CanvasGroup>(),1,0.5f);
        Time.timeScale = 0.2f;
        
    }

    public void Hint(string text)
    {
        hintText.text = text;
        LeanTween.alphaCanvas(hint, 1, 0.2f).setOnComplete(()=>
        {
            Time.timeScale = 0.1f;
            LeanTween.delayedCall(0.2f, () =>
            {
                Time.timeScale = 1;
                LeanTween.alphaCanvas(hint, 0, 0.2f);
            });
        });
        
    }

    private void OnApplicationQuit()
    {
        GlobalGameSettings.SaveProgress();
    }


}