using Sirenix.Utilities;

namespace BulwarkStudios.GameSystems.Libraries {

    [GlobalConfig(GameLibraryConstants.RESOURCE_GAMESYSTEM_LIBRARY_LIST_FOLDER)]
    public abstract class GameLibrary<T> : GlobalConfig<T>, IGameLibrary where T : GameLibrary<T>, new() {

        private bool isLoaded = false;

#if UNITY_EDITOR
        /// <summary>
        /// Editor initialization
        /// </summary>
        public static GameLibrary<T> EditorInitialize() {
            if (!Instance.isLoaded) {
                Instance.isLoaded = true;
            }
            return Instance;
        }
#endif

        #region Implementation of IGameContext

        /// <summary>
        /// Load the context
        /// </summary>
        void IGameLibrary.Load() {
            if (!Instance.isLoaded) {
                Instance.isLoaded = true;
            }
        }

        #endregion
    }

}
