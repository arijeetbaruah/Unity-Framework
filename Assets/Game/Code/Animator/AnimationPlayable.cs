using System;
using System.Collections.Generic;
using System.Linq;
using Baruah.Utils;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Baruah.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayable : MonoBehaviour
    {
        [SerializeField] private AnimationDictionray _clips;

        private PlayableGraph _playableGraph;
        private AnimationMixerPlayable _mixerPlayable;
        
        private Dictionary<string, int> _animationKeyToIndex = new();

        private void Awake()
        {
            if (_clips == null)
            {
                Debug.LogError("Animation clip is null");
                enabled = false;
                return;
            }

            foreach (var key in _clips.Keys.ToList())
            {
                if (_clips[key] == null)
                {
                    Debug.LogError($"Animation clip for key '{key}' is null", this);
                    _clips.Remove(key);
                }
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
            _mixerPlayable = AnimationMixerPlayable.Create(_playableGraph, _clips.Count);
            playableOutput.SetSourcePlayable(_mixerPlayable);
            
            int index = 0;
            foreach (var clip in _clips)
            {
                var clipPlayable = AnimationClipPlayable.Create(_playableGraph, clip.Value);
                // Connect the Playable to an output.
                _playableGraph.Connect(clipPlayable, 0, _mixerPlayable, index);
                _animationKeyToIndex.Add(clip.Key, index);
                index++;
            }

            // Plays the Graph.
            _playableGraph.Play();

            foreach (var key in _animationKeyToIndex.Keys)
            {
                SetWeight(key, 0);
            }
            
            SetWeight("idle", 1);
        }

        public void SetWeight(string key, float weight)
        {
            weight = Mathf.Clamp01(weight);
            if (_animationKeyToIndex.TryGetValue(key, out var index))
            {
                _mixerPlayable.SetInputWeight(index, weight);
            }
        }

        private void OnDestroy()
        {
            if (_playableGraph.IsValid())
            {
                _playableGraph.Destroy();
            }
        }
    }

    [System.Serializable]
    public class AnimationDictionray : SerializedDictionary<string, AnimationClip>
    {
        
    }
}
