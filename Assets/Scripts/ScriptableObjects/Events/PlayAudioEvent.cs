using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "PlayAudioEvent", menuName = "ScriptableObjects/Play Audio Event")]
    public class PlayAudioEvent : ScriptableObject
    {
        [NonSerialized] public UnityEvent<AudioClip> PlaySelectedAudioEvent;

        private void OnEnable()
        {
            PlaySelectedAudioEvent ??= new UnityEvent<AudioClip>();
        }

        public void PlaySelectedClip(AudioClip clip)
        {
            PlaySelectedAudioEvent.Invoke(clip);
        }
        
    }
}