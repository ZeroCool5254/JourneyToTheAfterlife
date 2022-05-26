using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerInputManagerSO", menuName = "ScriptableObjects/Player Input Manager")]
    public class PlayerInputManagerSO : ScriptableObject
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