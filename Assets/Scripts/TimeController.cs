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
        if (playbackMode)
        {
            foreach (TimeEntity e in timeEntities)
            {
                if(e != null)
                    e.Play(frame);
            }
            if(frame > 3)
            frame-=2;
        }
        else
        {
            foreach (TimeEntity e in timeEntities)
            {
                if (e != null)
                    e.Record(frame);
            }
            frame++;
        }
        
    }

}
