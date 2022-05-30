using System;
using Core;
using DefaultNamespace;
using ScriptableObjects.Events;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField, Header("AudioSettings")]
        private GameObject _audioSourcePrefab;

        [SerializeField, Header("Events")] private PlayAudioEvent _playAudioEvent;

        private void OnEnable()
        {
            _playAudioEvent.PlaySelectedAudioEvent.AddListener(PlayAudioClip);
        }

        private void OnDisable()
        {
            _playAudioEvent.PlaySelectedAudioEvent.RemoveListener(PlayAudioClip);
        }

        private void PlayAudioClip(AudioClip clip)
        {
            GameObject audioSource = Instantiate(_audioSourcePrefab, transform.position, Quaternion.identity);
            audioSource.GetComponent<AudioPlayer>().PlayAudioClip(clip);
        }
    }
}