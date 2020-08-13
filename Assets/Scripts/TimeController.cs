using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public GameObject padlockSprite;

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
            if(frame > 8)
            frame-=2;

            Player.instance.energy -= Time.unscaledDeltaTime / 2;
        }
        else
        {
            if (!Player.instance.gameObject.activeSelf) return;
            //fix for player disable issue

            foreach (TimeEntity e in timeEntities)
            {
                if (e != null)
                    e.Record(frame);
            }
            frame++;
        }
        
    }

}
