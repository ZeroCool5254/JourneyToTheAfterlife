using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "DialogueManagerSO", menuName = "ScriptableObjects/Dialogue Manager")]
    public class DialogueManagerSO : ScriptableObject
    {
        [NonSerialized] public UnityEvent<TextAsset> ActivateDialogueEvent;
        private void OnEnable()
        {
            ActivateDialogueEvent ??= new UnityEvent<TextAsset>();
        }
    }
}