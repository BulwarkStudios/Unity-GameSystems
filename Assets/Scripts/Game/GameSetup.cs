using UnityEngine;
using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Events;
using BulwarkStudios.GameSystems.Libraries;
using BulwarkStudios.GameSystems.Logs;

namespace BulwarkStudios.Tests {

    public class GameSetup : MonoBehaviour {

        /// <summary>
        /// Prepare
        /// </summary>
        private void Awake() {

            // Keep this object and its children alive
            DontDestroyOnLoad(this);

            // Load objects
            GameContextSystem.Load();
            GameEventSystem.Load();
            GameLibrarySystem.Load();

            // Init logs
            GameLogSystem.Initialize(new LogConfigTest());

        }

    }
}
