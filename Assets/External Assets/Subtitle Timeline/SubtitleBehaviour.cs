using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleBehaviour : PlayableBehaviour
{
    //public Subtitles subtitle;
    public string text;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Subtitles subtitle = playerData as Subtitles;
        if(subtitle != null)
        {
            subtitle.UpdateSub(text);
        }
    }

    
}
