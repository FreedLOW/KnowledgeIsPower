using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public HeroState HeroState;
        public HeroStats HeroStats;
        public KillData KillData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new HeroState();
            HeroStats = new HeroStats();
            KillData = new KillData();
        }
    }
}