using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        public void PlayAudioClip(AudioClip clip)
        {
            Debug.Log("AudioPlayer::Should play " + clip.name);
            _audioSource.PlayOneShot(clip);
            float delay = clip.length + 0.5f;
            Destroy(gameObject, delay);
        }
    }
}