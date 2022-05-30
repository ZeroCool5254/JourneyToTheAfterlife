using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "StartDialogueEvent", menuName = "ScriptableObjects/Dialogue Event")]
    public class StartDialogueEvent : ScriptableObject
    {
        [NonSerialized] public UnityEvent<TextAsset> ActivateDialogueEvent;
        private void OnEnable()
        {
            ActivateDialogueEvent ??= new UnityEvent<TextAsset>();
        }
    }
}