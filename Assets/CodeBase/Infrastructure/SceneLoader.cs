using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner CorutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            CorutineRunner = coroutineRunner;
        }

        public void Load(string sceneName, Action onSceneLoaded = null)
        {
            CorutineRunner.StartCoroutine(LoadScene(sceneName, onSceneLoaded));
        }
        
        private IEnumerator LoadScene(string sceneName, Action onSceneLoaded = null)
        {
            if (SceneManager.GetActiveScene().name.Equals(sceneName))
            {
                onSceneLoaded?.Invoke();
                yield break;
            }
            
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);

            while (!loadSceneAsync.isDone)
                yield return null;
            
            onSceneLoaded?.Invoke();
        }
    }
}