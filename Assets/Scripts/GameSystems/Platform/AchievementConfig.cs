using BulwarkStudios.GameSystems.Platform;

namespace Mechanicus.Config {

    public static class AchievementConfig {

        public static PlatformAchievement ACH_BASE_1 = new PlatformAchievement() {
            steamId = "ACH_BASE_1",
            originId = "1",
            description = "Survive the first mission",
        };

        /// <summary>
        /// Achievement unlock constraint depending on the difficulty settings
        /// </summary>
        /// <returns></returns>
        public static bool AchievementUnlockConstraint() {
            return false;
        }

    }

}