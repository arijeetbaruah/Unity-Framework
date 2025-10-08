using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class AnimationPlayable : MonoBehaviour
{
    [SerializeField] private AnimationClip _clip;
    
    private PlayableGraph _playableGraph;

    private void Awake()
    {
        if (_clip == null)
        {
            Debug.LogError("Animation clip is null");
            enabled = false;
        }
    }

    /// <summary>
    /// Creates a PlayableGraph, stores it in the <c>_playableGraph</c> field, and starts playback of the serialized <c>_clip</c> on this GameObject's <c>Animator</c> using game-time updates.
    /// </summary>
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

    private void OnDestroy()
    {
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }
}
