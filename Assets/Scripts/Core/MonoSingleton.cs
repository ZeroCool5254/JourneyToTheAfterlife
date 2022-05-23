using UnityEngine;

namespace Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        [Header("Singleton"), SerializeField, Tooltip("When used as a singleton, don't destroy on load")]
        protected bool dontDestroy;

        public static T Instance;

        protected virtual void Awake()
        {
            InitInstance();
        }

        /// <summary>
        /// Initialize the Instance/>
        /// </summary>
        private void InitInstance()
        {
            if (Instance == null) Instance = this as T;
            else Destroy(gameObject);

            if (dontDestroy) DontDestroyOnLoad(gameObject);
        }
    }
}