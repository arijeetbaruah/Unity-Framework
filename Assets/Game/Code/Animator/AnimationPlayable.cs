using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class AnimationPlayable : MonoBehaviour
{
    [SerializeField] private AnimationClip _clip;
    
    private PlayableGraph _playableGraph;

    private void Start()
    {
        _playableGraph = PlayableGraph.Create();
        _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        
        var playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", GetComponent<Animator>());
        
        // Wrap the clip in a playable.
        var clipPlayable = AnimationClipPlayable.Create(_playableGraph, _clip);

        // Connect the Playable to an output.
        playableOutput.SetSourcePlayable(clipPlayable);

        // Plays the Graph.
        _playableGraph.Play();
    }
}
