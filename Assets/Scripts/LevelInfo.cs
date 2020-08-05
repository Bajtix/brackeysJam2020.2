using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

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

    public float enemyCount;
    public float timeToComplete;


    public CanvasGroup panel, goalKills, goalTime, goalPortal;
    public TextMeshProUGUI timelimit, killlimit, levelname, leveldescription, health, energy;
    public GameObject crosshair;
    public float typeWriterSpeed = 0.1f;
    

    private void Start()
    {
        fader.alpha = 1;
        crosshair.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("LevelCamera").SetActive(true);
        LeanTween.alphaCanvas(fader, 0, 1);

        levelname.text = levelName;
        leveldescription.text = "";
        timelimit.text = "Time goal: " + timeToComplete + "s";
        killlimit.text = "Kill goal: " + enemyCount + " enemies";
        LeanTween.delayedCall(2, () =>
        {

            LeanTween.alphaCanvas(panel, 1, 2).setOnComplete(() =>
            {
                StartCoroutine("WriteDescription");
                LeanTween.delayedCall(levelDescription.Length * typeWriterSpeed + 0.3f, () =>
                {
                    StartCoroutine("CountEnergy");
                    LeanTween.delayedCall(1f / playerEnergy + 0.2f, () =>
                    {
                        StartCoroutine("CountHealth");
                        LeanTween.delayedCall(1f / playerHealth + 0.2f, () =>
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
                    Time();

                LeanTween.delayedCall(3, () =>
                {
                    LeanTween.alphaCanvas(fader, 1, 2).setOnComplete(()=> 
                    {
                        panel.alpha = 0;
                        Player.instance.gameObject.SetActive(true);
                        GameObject.FindGameObjectWithTag("LevelCamera").SetActive(false);
                        LeanTween.alphaCanvas(fader, 0, 2);
                        crosshair.SetActive(true);
                    });
                });
            });
        });
        
        
    }

    private void Kills()
    {
        LeanTween.alphaCanvas(goalKills, 1, 1);
    }

    private void Time()
    {
        LeanTween.alphaCanvas(goalTime, 1, 1);
    }

    private void Portal()
    {
        LeanTween.alphaCanvas(goalPortal, 1, 1);
    }
}