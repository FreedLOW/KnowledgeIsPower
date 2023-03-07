using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsListener
    {
        private const string AndroidGameId = "5194044";
        private const string IOSGameId = "5194045";
        private const string RewardedVideoPlacementId = "Rewarded_Android";
        
        private string gameId;
        private Action onVideoFinished;

        public bool IsRewardedVideoReady => Advertisement.IsReady(RewardedVideoPlacementId);
        public int Reward => 15;  //for test used this constant value in other way this value can be get from server or static data

        public event Action OnRewardedVideoReady;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    gameId = AndroidGameId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    gameId = IOSGameId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    gameId = AndroidGameId;
                    break;
            }
            
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId);
        }

        public void ShowRewardedVideo(Action videoFinishedAction)
        {
            Advertisement.Show(RewardedVideoPlacementId);
            onVideoFinished = videoFinishedAction;
        }

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"Ads ready {placementId}");

            if (placementId.Equals(RewardedVideoPlacementId))
                OnRewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message) => 
            Debug.Log($"Ads with error {message}");

        public void OnUnityAdsDidStart(string placementId) => 
            Debug.Log($"Ads start playing {placementId}");

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"ADS is failed {showResult}!");
                    break;
                case ShowResult.Skipped:
                    Debug.LogError($"ADS is skipped {showResult}!");
                    break;
                case ShowResult.Finished:
                    onVideoFinished?.Invoke();
                    break;
            }

            onVideoFinished = null;
        }
    }
}