using System;

namespace CodeBase.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        bool IsRewardedVideoReady { get; }
        int Reward { get; }
        event Action OnRewardedVideoReady;
        void Initialize();
        void ShowRewardedVideo(Action videoFinishedAction);
    }
}