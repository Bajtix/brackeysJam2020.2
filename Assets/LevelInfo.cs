using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public string levelName;
    [Multiline]
    public string levelDescription;

    public bool eliminateEnemies;
    public bool time;

    public float playerHealth;
    public float playerEnergy;

    public float enemyCount;
    public float timeToComplete;
}