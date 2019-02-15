using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    [RequireComponent(typeof(UiButton))]
    public abstract class UiButtonKeyboardControl : UiButtonControl {

        /// <summary>
        /// Update the control
        /// </summary>
        protected override void UpdateControl() {

            UpdateKeyboard();

        }

        /// <summary>
        /// Update the keyboard
        /// </summary>
        protected abstract void UpdateKeyboard();

    }
}