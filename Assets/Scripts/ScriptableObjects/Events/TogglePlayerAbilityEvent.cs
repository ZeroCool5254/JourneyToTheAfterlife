using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "TogglePlayerAbilityEvent", menuName = "ScriptableObjects/Player Ability Event")]
    public class TogglePlayerAbilityEvent : ScriptableObject
    {
        public bool AbilityEnabled { get; private set; }
        [NonSerialized] public UnityEvent<bool> AbilityEnabledEvent;

        private void OnEnable()
        {
            AbilityEnabledEvent ??= new UnityEvent<bool>();
        }

        public void EnableAbility()
        {
            AbilityEnabled = true;
            AbilityEnabledEvent.Invoke(AbilityEnabled);
        }

        public void DisableAbility()
        {
            AbilityEnabled = false;
            AbilityEnabledEvent.Invoke(AbilityEnabled);
        }
    }
}