using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    public class UiButtonKeyboardControlButton : UiButtonKeyboardControl {

        /// <summary>
        /// Need the control active?
        /// </summary>
        [SerializeField]
        private bool needControlActive = false;

        /// <summary>
        /// Key code
        /// </summary>
        [SerializeField]
        private KeyCode keyCode = KeyCode.Escape;

        /// <summary>
        /// Control active?
        /// </summary>
        /// <returns></returns>
        protected override bool IsControlActive() {
            if (needControlActive) {
                return base.IsControlActive();
            }

            return true;
        }

        /// <summary>
        /// Update the keyboard
        /// </summary>
        protected override void UpdateKeyboard() {

            // Key input?
            if (!Input.GetKeyUp(keyCode)) {
                return;
            }

            // Trigger
            Button.Submit();

        }


    }
}