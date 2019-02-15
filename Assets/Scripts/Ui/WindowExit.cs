using UnityEngine;
using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Utils.EventTriggers;

namespace BulwarkStudios.Tests {

    public class WindowExit : MonoBehaviour, IPreparable {

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
            GameEventExitClose.Listen(this, ExitClose).AddGameContextConstraint(GameContextExit.Instance);

        }

        private void OnDisable() {

            // Unlisten an event
            GameEventExitClose.Unlisten(this, ExitClose);

        }

        /// <summary>
        /// Open
        /// </summary>
        public void Open() {

            // Set a context and add a layer
            GameContextSystem.AddLayer(GameContextExit.Instance, GameContextSystem.INDEX.MAIN);
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

        private void ExitClose() {

            Close();

        }

    }
}
