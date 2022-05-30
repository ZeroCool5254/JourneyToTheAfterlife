using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "EndDialogueEvent", menuName = "ScriptableObjects/Dialogue Finished Event")]
    public class EndDialogueEvent : ScriptableObject
    {
        [NonSerialized] public UnityEvent CompleteDialogueEvent;

        private void OnEnable()
        {
            CompleteDialogueEvent ??= new UnityEvent();
        }
    }
}
