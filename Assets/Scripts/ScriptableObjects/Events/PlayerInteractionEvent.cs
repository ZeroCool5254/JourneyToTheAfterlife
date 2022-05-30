using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "PlayerInteractionEvent", menuName = "ScriptableObjects/Player Interaction Event")]
    public class PlayerInteractionEvent : ScriptableObject
    {
        [NonSerialized] public UnityEvent InteractionEvent;

        private void OnEnable()
        {
            InteractionEvent ??= new UnityEvent();
        }
    }
}