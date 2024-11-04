using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameId = "AndroidGameId";
        private const string IOSGameId = "IOSGameId";

        private const string RewardedVideoPlacementId = "RewardedVideoPlacementId";
        
        private string _gameId;
        private Action _onVideoFinished;

        public event Action OnRewardedVideoReady;

        public int Reward => 20;

        public void Initialize()
        {
            _gameId = Application.platform switch
            {
                RuntimePlatform.Android => AndroidGameId,
                RuntimePlatform.IPhonePlayer => IOSGameId,
                RuntimePlatform.WindowsEditor => AndroidGameId,
                _ => _gameId
            };
            
            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(RewardedVideoPlacementId);
            _onVideoFinished = onVideoFinished;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"OnUnityAdsAdLoaded: {placementId}");
            
            if (placementId == RewardedVideoPlacementId)
                OnRewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) => 
            Debug.LogError($"OnUnityAdsFailedToLoad: {placementId}, error: {error}, message: {message}");

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) => 
            Debug.LogError($"OnUnityAdsFailedToLoad: {placementId}, error: {error}, message: {message}");

        public void OnUnityAdsShowStart(string placementId) => 
            Debug.Log($"OnUnityAdsShowStart: {placementId}");

        public void OnUnityAdsShowClick(string placementId) => 
            Debug.Log($"OnUnityAdsShowClick: {placementId}");

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.LogWarning("Video was skipped");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    _onVideoFinished?.Invoke();
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                default:
                    Debug.LogError($"Unknown result with placement id:{placementId}");
                    break;
            }

            _onVideoFinished = null;
        }
    }
}