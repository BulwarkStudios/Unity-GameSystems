using BulwarkStudios.GameSystems.Contexts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    [RequireComponent(typeof(UiButton))]
    public class UiButtonConstraintContext : UiButtonConstraint {

        /// <summary>
        /// Context
        /// </summary>
        [SerializeField, AssetList(Path = GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER)]
        private ScriptableObject context = null;

        /// <summary>
        /// Game context
        /// </summary>
        private IGameContext gameContext;

        /// <summary>
        /// Get the game context
        /// </summary>
        private IGameContext GameContext {
            get {
                if (gameContext == null) {
                    gameContext = context as IGameContext;
                }

                return gameContext;
            }
        }

        /// <summary>
        /// Event
        /// </summary>
        private void OnEnable() {
            GameContextSystem.OnUpdateContext += OnUpdateContext;
        }

        /// <summary>
        /// Event
        /// </summary>
        private void OnDisable() {
            GameContextSystem.OnUpdateContext -= OnUpdateContext;
        }

        /// <summary>
        /// Set the context
        /// </summary>
        /// <param name="context"></param>
        public void SetGameContext(IGameContext context) {
            gameContext = context;
            OnUpdateContext();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnUpdateContext() {
            RefreshButtonState();
        }

        /// <summary>
        /// The contraint is valid?
        /// </summary>
        /// <returns></returns>
        public override bool IsValid() {
            if (GameContext == null) {
                return false;
            }
            if (GameContextSystem.HasContext(GameContext)) {
                return true;
            }
            return false;
        }

    }

}
