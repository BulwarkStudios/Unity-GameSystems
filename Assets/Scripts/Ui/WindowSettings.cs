using UnityEngine;
using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Utils.EventTriggers;

namespace BulwarkStudios.Tests {

    public class WindowSettings : MonoBehaviour, IPreparable {

        #region Implementation of IPreparable

        /// <summary>
        /// Prepare the object
        /// </summary>
        public void Prepare() {
            gameObject.SetActive(false);
        }

        #endregion

        private void OnEnable() {

            // Listen an event
            GameEventSettingsClose.Listen(this, SettingsClose).AddGameContextConstraint(GameContextSettings.Instance);

        }

        private void OnDisable() {

            // Unlisten an event
            GameEventSettingsClose.Unlisten(this, SettingsClose);

        }

        /// <summary>
        /// Open
        /// </summary>
        public void Open() {

            // Set a context and add a layer
            GameContextSystem.AddLayer(GameContextSettings.Instance, GameContextSystem.INDEX.MAIN);
            gameObject.SetActive(true);

        }

        /// <summary>
        /// Close
        /// </summary>
        private void Close() {

            // Remove context layer
            GameContextSystem.RemoveLayer();
            gameObject.SetActive(false);

        }

        private void SettingsClose() {

            Close();

        }

    }
}
