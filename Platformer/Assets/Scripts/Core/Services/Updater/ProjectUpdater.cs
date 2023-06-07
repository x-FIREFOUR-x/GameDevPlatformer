using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Services.Updater
{
    public class ProjectUpdater : MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        
        private bool _isPaused = true;
        private readonly List<Coroutine> _invokedCoroutines = new List<Coroutine>();
        
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
        
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled;
        public event Action LateUpdateCalled;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(this.gameObject);
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
        
        private void OnDestroy()
        {
            foreach (var element in _invokedCoroutines.Where(element => element != null))
                StopCoroutine(element);
            
            _invokedCoroutines.Clear();
        }

        public void Invoke(Action action, float timeDelay)
        {
            _invokedCoroutines.Add(StartCoroutine(InvokeCoroutine(action, timeDelay)));
        }
        
        Coroutine IProjectUpdater.StartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
        void IProjectUpdater.StopCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);
        
        private IEnumerator InvokeCoroutine(Action action, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            yield return new WaitUntil(() => !IsPaused);
            action?.Invoke();
            _invokedCoroutines.RemoveAll(element => element == null);
        }
    }
}
