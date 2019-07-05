using UnityEngine;
using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Events;
using BulwarkStudios.GameSystems.Libraries;
using BulwarkStudios.GameSystems.Logs;
using BulwarkStudios.GameSystems.Platform;
using Mechanicus.Config;

namespace BulwarkStudios.Tests {

    public class GameSetup : MonoBehaviour {

        /// <summary>
        /// Prepare
        /// </summary>
        private void Awake() {

            // Keep this object and its children alive
            DontDestroyOnLoad(this);

            // Initialize the platform
#if GOOGLE_PLAY_BUILD
            Platform.Initialize(new GooglePlay(), new GooglePlayConfig());
#elif STEAM_BUILD
            Platform.Initialize(new Steam(), new SteamConfig());
#elif ORIGIN_BUILD
            Platform.Initialize(new BulwarkStudios.GameSystems.Platform.Origin(), new OriginConfig());
#elif SWITCH_BUILD
            Platform.Initialize(new Switch(), new OriginConfig());
#elif PS4_BUILD
            Platform.Initialize(new Ps4(), new Ps4Config());
#elif XBOXONE_BUILD
            Platform.Initialize(new XboxOne(), new XboxOneConfig());
#elif EDITOR_BUILD
            Platform.Initialize(new Unity(), new UnityConfig());
#endif

            // Setup platform
            Platform.Get().SetAchievementUnlockConstraint(AchievementConfig.AchievementUnlockConstraint);

            // Load objects
            GameContextSystem.Load();
            GameEventSystem.Load();
            GameLibrarySystem.Load();

            // Init logs
            GameLogSystem.Initialize(new LogConfigTest());

        }

    }
}
