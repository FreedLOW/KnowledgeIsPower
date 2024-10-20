using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string sceneName, Action onSceneLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(sceneName, onSceneLoaded));
        }
        
        private IEnumerator LoadScene(string sceneName, Action onSceneLoaded = null)
        {
            if (SceneManager.GetActiveScene().name.Equals(sceneName))
            {
                onSceneLoaded?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);

            while (!waitNextScene.isDone)
                yield return null;
            
            onSceneLoaded?.Invoke();
        }
    }
}