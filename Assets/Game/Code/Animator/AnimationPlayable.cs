using System;
using System.Collections.Generic;
using System.Linq;
using Baruah.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Baruah.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayable : MonoBehaviour
    {
        [SerializeField] private AnimationDictionary _clips;
        [ValueDropdown(nameof(GetKeys))]
        [SerializeField] private string _defaultAnimationKey = "idle";

        private PlayableGraph _playableGraph;
        private AnimationMixerPlayable _mixerPlayable;
        
        private Dictionary<string, int> _animationKeyToIndex = new();

        private IEnumerable<string> GetKeys => _clips.Keys;

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

            if (_clips.Count == 0)
            {
                Debug.LogError("No valid animation clips found after cleanup", this);
                enabled = false;
                return;
            }

            if (string.IsNullOrEmpty(_defaultAnimationKey) || !_clips.ContainsKey(_defaultAnimationKey))
            {
                Debug.LogError($"Default animation key '{_defaultAnimationKey}' not found in clips dictionary", this);
                enabled = false;
                return;
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
            
            SetWeight(_defaultAnimationKey, 1);
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
    public class AnimationDictionary : SerializedDictionary<string, AnimationClip>
    {
        
    }
}
