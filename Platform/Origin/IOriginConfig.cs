namespace BulwarkStudios.GameSystems.Platform {

    public interface IOriginConfig : IPlatformConfig {

        /// <summary>
        /// Get the sekurity key
        /// </summary>
        /// <returns></returns>
        string GetSecurityKey();

        /// <summary>
        /// Get the app id
        /// </summary>
        /// <returns></returns>
        string GetAppId();

        /// <summary>
        /// Get the achievement set id
        /// </summary>
        /// <returns></returns>
        string GetAchievementSetId();

        /// <summary>
        /// Get the achievement code
        /// </summary>
        /// <returns></returns>
        string GetAchievementCode();

    }

}