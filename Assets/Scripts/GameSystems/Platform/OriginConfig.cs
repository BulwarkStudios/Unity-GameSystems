using BulwarkStudios.GameSystems.Platform;

namespace Mechanicus.Config {

    public class OriginConfig : IOriginConfig {

        /// <summary>
        /// Origin security key
        /// </summary>
        private const string SECURITY_KEY = "{82448BC5-282B-4FDF-A862-3E3D9C6833E2}";

        /// <summary>
        /// Origin App Id
        /// </summary>
        private const string APP_ID = "198142";

        /// <summary>
        /// Origin achievement set id
        /// </summary>
        private const string ACHIEVEMENT_SET_ID = "50318_198142_50844";

        /// <summary>
        /// Origin achievement code
        /// </summary>
        private const string ACHIEVEMENT_CODE = "10ddde89-cf68-44d7-91da-0fc27048a263";

        #region Implementation of IPlatformConfig

        /// <summary>
        /// Get the sekurity key
        /// </summary>
        /// <returns></returns>
        string IOriginConfig.GetSecurityKey() {
            return SECURITY_KEY;
        }

        /// <summary>
        /// Get the app id
        /// </summary>
        /// <returns></returns>
        string IOriginConfig.GetAppId() {
            return APP_ID;
        }

        /// <summary>
        /// Get the achievement set id
        /// </summary>
        /// <returns></returns>
        string IOriginConfig.GetAchievementSetId() {
            return ACHIEVEMENT_SET_ID;
        }

        /// <summary>
        /// Get the achievement code
        /// </summary>
        /// <returns></returns>
        string IOriginConfig.GetAchievementCode() {
            return ACHIEVEMENT_CODE;
        }

        #endregion

    }

}