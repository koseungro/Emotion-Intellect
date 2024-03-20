using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SubtitleControlBehaviour : PlayableBehaviour
{

    public AudioClip clip;


    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //if (clip != null)
        //{
        //    Debug.Log(clip.name + " : �̸���");
        //}

    }

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (clip != null)
        {
            SubtitleManager.Instance.SubtitleSet(clip.name);
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

        if (clip != null)
        {
            SubtitleManager.Instance.TextSetNull();
            SubtitleManager.Instance.SubtitleSet("null");
        }

    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
