using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "TogglePlayerInputEvent", menuName = "ScriptableObjects/Player Input Event")]
    public class TogglePlayerInputEvent : ScriptableObject
    {
        public bool InputEnabled { get; private set; }
        [NonSerialized] public UnityEvent<bool> InputChangedEvent;

        private void OnEnable()
        {
            InputChangedEvent ??= new UnityEvent<bool>();
        }

        public void EnableInput()
        {
            InputEnabled = true;
            InputChangedEvent.Invoke(InputEnabled);
        }

        public void DisableInput()
        {
            InputEnabled = false;
            InputChangedEvent.Invoke(InputEnabled);
        }
    }
}