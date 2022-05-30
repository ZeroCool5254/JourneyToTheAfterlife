using System;
using System.Collections;
using Core;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoadManager : MonoSingleton<SceneLoadManager>
    {
        [SerializeField, Header("UI")] private GameObject _loadingScreen;
        [SerializeField, Header("Events")] private LoadSceneEvent _loadSceneEvent;

        private void OnEnable()
        {
            _loadingScreen.SetActive(false);
            _loadSceneEvent.LoadSelectedSceneEvent.AddListener(LoadLevel);
        }

        private void OnDisable()
        {
            _loadSceneEvent.LoadSelectedSceneEvent.RemoveListener(LoadLevel);
        }

        public void LoadLevel(string levelName)
        {
            _loadingScreen.SetActive(true);
            StartCoroutine(LoadSelectedSceneRoutine(levelName));
        }

        private IEnumerator LoadSelectedSceneRoutine(string levelName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}