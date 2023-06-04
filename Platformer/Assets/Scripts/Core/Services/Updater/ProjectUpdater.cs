using System;
using System.Collections;
using UnityEngine;

namespace Core.Services.Updater
{
    public class ProjectUpdater : MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled;
        public event Action LateUpdateCalled;

        private bool _isPaused = true;
        public bool IsPaused { 
            get => _isPaused;
            set 
            {
                if (_isPaused == value)
                    return;

                Time.timeScale = value ? 0 : 1;
                _isPaused = value;
            } 
        }

        Coroutine IProjectUpdater.StartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
        void IProjectUpdater.StopCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (IsPaused)
                return;

            UpdateCalled?.Invoke();
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            FixedUpdateCalled?.Invoke();
        }

        private void LateUpdate()
        {
            if (IsPaused)
                return;

            LateUpdateCalled?.Invoke();
        }
    }
}
