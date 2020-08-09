using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(SubtitleAsset))]
[TrackBindingType(typeof(Subtitles))]
public class SubtitleTrack : TrackAsset 
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        foreach(TimelineClip t in this.GetClips())
        {
            t.displayName = ((SubtitleAsset)t.asset).text;
        }

        return base.CreateTrackMixer(graph, go, inputCount);
    }
}

