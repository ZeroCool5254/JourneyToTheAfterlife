using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "UpdateManaEvent", menuName = "ScriptableObjects/Mana Changed Event")]
    public class UpdateManaEvent : ScriptableObject
    {
        public float Mana { get; private set; }
        public float MaxMana;
        
        [NonSerialized] public UnityEvent<float> ManaChangedEvent;

        private void OnEnable()
        {
            ManaChangedEvent ??= new UnityEvent<float>();
        }

        public void SetMana()
        {
            Mana = MaxMana;
            ManaChangedEvent.Invoke(Mana);
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
