using System;
using System.Collections;
using UnityEngine;

namespace CodeBase
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
        }

        public void Hide() => StartCoroutine(HideRoutine());

        private IEnumerator HideRoutine()
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= 0.03f;
                yield return new WaitForEndOfFrame();
            }
            
            gameObject.SetActive(false);
        }
    }
}