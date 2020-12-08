using BulwarkStudios.GameSystems.Utils;

namespace BulwarkStudios.GameSystems.Libraries {

    public abstract class GameLibrary<T> : ScriptableObjectSingleton<T>, IGameLibrary where T : GameLibrary<T>, new() {

        /// <summary>
        /// Get the instance
        /// </summary>
        public new static T Instance {
            get {
#if GAMESYSTEMS_AUTO_LOAD
                GameLibrarySystem.Load();
#endif
                return instance;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor initialization
        /// </summary>
        public static GameLibrary<T> EditorInitialize() {
            return CreateSingleton(GameLibraryConstants.RESOURCE_GAMESYSTEM_LIBRARY_LIST_FOLDER);
        }
#endif

    }

}