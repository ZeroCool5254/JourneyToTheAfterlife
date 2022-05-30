using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "UpdateHealthEvent", menuName = "ScriptableObjects/Health Changed Event")]
    public class UpdateHealthEvent : ScriptableObject
    {
        public int Health { get; private set; }
        public int MaxHealth = 6;

        [NonSerialized] public UnityEvent<int> HealthChangedEvent;
        
        private void OnEnable()
        {
            HealthChangedEvent ??= new UnityEvent<int>();
        }

        public void SetHealth()
        {
            Health = MaxHealth;
            HealthChangedEvent.Invoke(Health);
        }

        public void IncreaseHealth()
        {
            if (!(Health < MaxHealth)) return;
            Health++;
            HealthChangedEvent.Invoke(Health);
        }

        public void DecreaseHealth()
        {
            if (!(Health > 0)) return;
            Health--;
            HealthChangedEvent.Invoke(Health);
        }
    }
}
