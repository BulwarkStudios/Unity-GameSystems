using Sirenix.Utilities;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Contexts {

    [GlobalConfig(GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER)]
    public abstract class GameContext<T> : GlobalConfig<T>, IGameContext where T : GameContext<T>, new() {

        private bool isLoaded = false;

#if UNITY_EDITOR
        /// <summary>
        /// Editor initialization
        /// </summary>
        public static GameContext<T> EditorInitialize() {
            if (!Instance.isLoaded) {
                Instance.isLoaded = true;
            }
            return Instance;
        }
#endif

        /// <summary>
        /// Context enabled
        /// </summary>
        /// <param name="index"></param>
        protected virtual void Enable(GameContextSystem.INDEX index) { }

        /// <summary>
        /// Context disabled
        /// </summary>
        /// <param name="index"></param>
        protected virtual void Disable(GameContextSystem.INDEX index) { }

        #region Implementation of IGameContext

        /// <summary>
        /// Get the scriptable object
        /// </summary>
        /// <returns></returns>
        ScriptableObject IGameContext.GetScriptableObject() {
            return this;
        }

        /// <summary>
        /// Load the context
        /// </summary>
        void IGameContext.Load() {
            if (!Instance.isLoaded) {
                Instance.isLoaded = true;
            }
        }

        /// <summary>
        /// Context enabled
        /// </summary>
        /// <param name="index"></param>
        void IGameContext.Enable(GameContextSystem.INDEX index) {
            Enable(index);
        }

        /// <summary>
        /// Context disabled
        /// </summary>
        /// <param name="index"></param>
        void IGameContext.Disable(GameContextSystem.INDEX index) {
            Disable(index);
        }

        #endregion
    }

}
