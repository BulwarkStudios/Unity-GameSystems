namespace BulwarkStudios.GameSystems.Platform {

    public interface ISteamConfig : IPlatformConfig {

        /// <summary>
        /// Get the app id
        /// </summary>
        /// <returns></returns>
        uint GetAppId();

    }

}