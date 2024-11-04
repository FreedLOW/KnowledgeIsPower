using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        public Button RewardButton;
        public GameObject[] AddActiveObjects;
        public GameObject[] AddInactiveObjects;
        
        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }
        
        public void Initialize()
        {
            RewardButton.onClick.AddListener(OnShowRewardedAd);

            RefreshAvailableAd();
        }

        public void Subscribe() => 
            _adsService.OnRewardedVideoReady += RefreshAvailableAd;

        public void Cleanup() => 
            _adsService.OnRewardedVideoReady -= RefreshAvailableAd;

        private void OnShowRewardedAd() => 
            _adsService.ShowRewardedVideo(OnVideoFinished);

        private void RefreshAvailableAd()
        {
            foreach (GameObject addActiveObject in AddActiveObjects)
            {
                addActiveObject.SetActive(true);
            }

            foreach (GameObject addInactiveObject in AddInactiveObjects)
            {
                addInactiveObject.SetActive(false);
            }
        }

        private void OnVideoFinished() => 
            _progressService.PlayerProgress.WorldData.LootData.Add(_adsService.Reward);
    }
}