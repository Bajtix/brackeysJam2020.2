using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    #region Singleton
    public static TimeController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }
    #endregion

    public ulong frame = 0;
    public bool playbackMode = false;
    public float smoothingSpeed = 10;

    public List<TimeEntity> timeEntities;


    private void FixedUpdate()
    {
        playbackMode = Input.GetKey(KeyCode.F);
        if (playbackMode)
        {
            foreach (TimeEntity e in timeEntities)
            {
                e.Play(frame);
            }
            frame--;
        }
        else
        {
            foreach (TimeEntity e in timeEntities)
            {          
                e.Record(frame);
            }
            frame++;
        }
        
    }

}
