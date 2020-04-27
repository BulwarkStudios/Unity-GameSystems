using System.Collections.Generic;
using BulwarkStudios.GameSystems.Utils;
using Sirenix.OdinInspector;
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

        /// <summary>
        /// Behaviours
        /// </summary>
        [ShowInInspector, ReadOnly]
        private List<GameContextBehaviour> behaviours;

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

        /// <summary>
        /// Setup behaviours
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<GameContextBehaviour> SetupBehaviours() {
            yield break;
        }

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

            // Enbale behaviours
            if (behaviours == null) {
                behaviours = new List<GameContextBehaviour>(SetupBehaviours());
            }

            foreach (GameContextBehaviour behaviour in behaviours) {
                behaviour.Enable(index);
            }

            Enable(index);
        }

        /// <summary>
        /// Context disabled
        /// </summary>
        /// <param name="index"></param>
        void IGameContext.Disable(GameContextSystem.INDEX index) {

            // Disable behaviours
            if (behaviours == null) {
                behaviours = new List<GameContextBehaviour>(SetupBehaviours());
            }

            foreach (GameContextBehaviour behaviour in behaviours) {
                behaviour.Disable(index);
            }

            Disable(index);
        }

        /// <summary>
        /// Context disabled with remove layer
        /// </summary>
        void IGameContext.RemoveLayer() {

            // Enbale behaviours
            if (behaviours == null) {
                behaviours = new List<GameContextBehaviour>(SetupBehaviours());
            }

            foreach (GameContextBehaviour behaviour in behaviours) {
                behaviour.RemoveLayer();
            }

        }

        /// <summary>
        /// Clear the context
        /// </summary>
        void IGameContext.Clear() {

            foreach (GameContextBehaviour behaviour in behaviours) {
                behaviour.Clear();
            }
            behaviours.Clear();

        }

        #endregion

    }

}