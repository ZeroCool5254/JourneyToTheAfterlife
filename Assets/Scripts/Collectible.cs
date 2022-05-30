using System;
using ScriptableObjects.Events;
using Characters.Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField, Header("Events")] private UpdateHealthEvent _healthEvent;
        [SerializeField] private UpdateManaEvent _manaEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _healthEvent.IncreaseHealth();
                _manaEvent.IncreaseMana(3);
                Destroy(gameObject);
            }
        }
    }
}