using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "AbilityManagerSO", menuName = "ScriptableObjects/Ability Manager")]
    public class ManaManagerSO : ScriptableObject
    {
        public float Mana;
        public float MaxMana;
        
        [NonSerialized] public UnityEvent<float> ManaChangedEvent;

        private void OnEnable()
        {
            Mana = MaxMana;
            ManaChangedEvent ??= new UnityEvent<float>();
        }

        public void IncreaseMana(float amount)
        {
            if (!(Mana < MaxMana)) return;
            Mana += amount;
            ManaChangedEvent.Invoke(Mana);
        }

        public void DecreaseMana(float amount)
        {
            if (!(Mana > 0)) return;
            Mana -= amount;
            ManaChangedEvent.Invoke(Mana);
        }
    }
}
