using BulwarkStudios.GameSystems.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    [RequireComponent(typeof(UiButton))]
    public class UiButtonTrigger : MonoBehaviour {

        /// <summary>
        /// Ref to the button
        /// </summary>
        private UiButton refButton;

        /// <summary>
        /// Get the button
        /// </summary>
        private UiButton Button {
            get {
                if (refButton == null) {
                    refButton = GetComponent<UiButton>();
                }

                return refButton;
            }
        }

        /// <summary>
        /// Trigger event
        /// </summary>
        [SerializeField, AssetList(Path = GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_LIST_FOLDER)]
        private ScriptableObject triggerEvent = null;

        /// <summary>
        /// Game event
        /// </summary>
        private IGameEvent gameEvent;

        /// <summary>
        /// Get the game event
        /// </summary>
        private IGameEvent GameEvent {
            get {
                if (gameEvent == null) {
                    gameEvent = triggerEvent as IGameEvent;
                }

                return gameEvent;
            }
        }

        /// <summary>
        /// Event
        /// </summary>
        private void OnEnable() {
            Button.OnTriggered += OnTrigger;
            GameEvent?.Listen(this);
        }

        /// <summary>
        /// Event
        /// </summary>
        private void OnDisable() {
            Button.OnTriggered -= OnTrigger;
            GameEvent?.Unlisten(this);
        }

        /// <summary>
        /// Trigger
        /// </summary>
        private void OnTrigger() {
            GameEvent?.Trigger();
        }

    }

}