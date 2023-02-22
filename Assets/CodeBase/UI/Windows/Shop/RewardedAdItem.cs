using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        public Button showRewardAdButton;
        public GameObject[] adActiveObjects;
        public GameObject[] adInactiveObjects;
        
        private IAdsService adsService;
        private IPersistentProgressService progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            this.adsService = adsService;
            this.progressService = progressService;
        }

        public void Initialize()
        {
            showRewardAdButton.onClick.AddListener(ShowRewardedAd);
            RefreshAvailableAd();
        }

        public void Subscribe() => 
            adsService.OnRewardedVideoReady += RefreshAvailableAd;

        public void Cleanup() => 
            adsService.OnRewardedVideoReady -= RefreshAvailableAd;

        private void ShowRewardedAd() => 
            adsService.ShowRewardedVideo(OnRewardedVideoFinished);

        private void OnRewardedVideoFinished() => 
            progressService.PlayerProgress.WorldData.LootData.Add(adsService.Reward);

        private void RefreshAvailableAd()
        {
            bool isRewardedVideoReady = adsService.IsRewardedVideoReady;

            foreach (GameObject adActiveObject in adActiveObjects)
            {
                adActiveObject.SetActive(isRewardedVideoReady);
            }

            foreach (GameObject adInactiveObject in adInactiveObjects)
            {
                adInactiveObject.SetActive(!isRewardedVideoReady);
            }
        }
    }
}