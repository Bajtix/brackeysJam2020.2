using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleAsset : PlayableAsset
{
    //public ExposedReference<Subtitles> sub;
    public string text;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        //lightControlBehaviour.subtitle = sub.Resolve(graph.GetResolver());
        behaviour.text = text;
        return playable;
    }
}
