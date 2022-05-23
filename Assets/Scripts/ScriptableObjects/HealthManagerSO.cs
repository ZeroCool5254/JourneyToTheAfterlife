using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealthManagerSO", menuName = "ScriptableObjects/Health Manager")]
    public class HealthManagerSO : ScriptableObject
    {
        public int Health;
        public int MaxHealth = 6;

        [NonSerialized] public UnityEvent<int> HealthChangedEvent;
        
        private void OnEnable()
        {
            Health = MaxHealth;
            HealthChangedEvent ??= new UnityEvent<int>();
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
