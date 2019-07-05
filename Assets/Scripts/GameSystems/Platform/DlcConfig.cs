using BulwarkStudios.GameSystems.Platform;

namespace Mechanicus.Config {

    public static class DlcConfig {

        /// <summary>
        /// DLC Omnissiah edition
        /// </summary>
        public static PlatformDlc DLC_OMNISSIAH_EDITION = new PlatformDlc() {
            steamAppDlcId = 916250,
            originEntitlementTag = "WARHAMMER_40K_MECH_ASW",
            hasInEditor = true,
        };

    }

}