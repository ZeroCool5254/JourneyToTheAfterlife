using System;
using ScriptableObjects.Events;
using Characters.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField, Header("Audio")] private AudioClip[] _collectedClips;
        
        [SerializeField, Header("Events")] private UpdateHealthEvent _healthEvent;
        [SerializeField] private UpdateManaEvent _manaEvent;
        [SerializeField] private PlayAudioEvent _playAudioEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                int selectedClip = Random.Range(0, _collectedClips.Length);
                _playAudioEvent.PlaySelectedClip(_collectedClips[selectedClip]);
                _healthEvent.IncreaseHealth();
                _manaEvent.IncreaseMana(3);
                Destroy(gameObject);
            }
        }
    }
}