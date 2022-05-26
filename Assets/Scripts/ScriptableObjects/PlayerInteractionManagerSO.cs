using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerInteractionManagerSO", menuName = "ScriptableObjects/Player Interaction Manager")]
    public class PlayerInteractionManagerSO : ScriptableObject
    {
        [NonSerialized] public UnityEvent InteractionEvent;

        private void OnEnable()
        {
            InteractionEvent ??= new UnityEvent();
        }
    }
}