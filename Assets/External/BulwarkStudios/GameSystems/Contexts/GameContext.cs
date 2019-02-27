using BulwarkStudios.GameSystems.Utils;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Contexts {

    public abstract class GameContext<T> : ScriptableObjectSingleton<T>, IGameContext where T : GameContext<T>, new() {

        /// <summary>
        /// Get the instance
        /// </summary>
        public new static T Instance {
            get {
                GameContextSystem.Load();
                return instance;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor initialization
        /// </summary>
        public static GameContext<T> EditorInitialize() {
            return CreateSingleton(GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER);
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
