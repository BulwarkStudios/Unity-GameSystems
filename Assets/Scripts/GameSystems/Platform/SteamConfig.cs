using BulwarkStudios.GameSystems.Platform;

namespace Mechanicus.Config {

    public class SteamConfig : ISteamConfig {

        /// <summary>
        /// App id steam
        /// </summary>
        private const uint APP_ID = 673880;

        #region Implementation of IPlatformConfig

        /// <summary>
        /// Get the app id
        /// </summary>
        /// <returns></returns>
        uint ISteamConfig.GetAppId() {
            return APP_ID;
        }

        #endregion

    }

}