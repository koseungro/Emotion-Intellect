using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class SubtitleAsset : PlayableAsset
{
    public ExposedReference<AudioClip> audioclip;


    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SubtitleControlBehaviour>.Create(graph);

        var audio = playable.GetBehaviour();
        audio.clip = audioclip.Resolve(graph.GetResolver());

        //var subtitleBehaviour = playable.GetBehaviour();

        return playable;
    }
}
