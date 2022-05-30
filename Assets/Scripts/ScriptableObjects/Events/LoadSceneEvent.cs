using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "LoadSceneEvent", menuName = "ScriptableObjects/Load Scene Event")]
    public class LoadSceneEvent : ScriptableObject
    {
        [NonSerialized] public UnityEvent<string> LoadSelectedSceneEvent;

        private void OnEnable()
        {
            LoadSelectedSceneEvent ??= new UnityEvent<string>();
        }
    }
}