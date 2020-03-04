using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    [RequireComponent(typeof(UiButton))]
    public abstract class UiButtonControl : MonoBehaviour {

        /// <summary>
        /// Ref to the button
        /// </summary>
        private UiButton refButton;

        /// <summary>
        /// Get the button
        /// </summary>
        protected UiButton Button {
            get {
                if (refButton == null) {
                    refButton = GetComponent<UiButton>();
                }

                return refButton;
            }
        }

        /// <summary>
        /// Control active?
        /// </summary>
        private bool controlActive;

        /// <summary>
        /// Enable
        /// </summary>
        protected virtual void OnEnable() {
            Button.OnStateUpdated += OnStateUpdated;
        }

        /// <summary>
        /// Disable
        /// </summary>
        protected virtual void OnDisable() {
            Button.OnStateUpdated -= OnStateUpdated;
        }

        /// <summary>
        /// Button state updated
        /// </summary>
        /// <param name="state"></param>
        private void OnStateUpdated(UiButton.STATE state) {
            if (state == UiButton.STATE.HIGHLIGHTED) {
                controlActive = true;
            }
            else {
                controlActive = false;
            }

            OnButtonStateUpdated(state);
        }

        /// <summary>
        /// Control active?
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsControlActive() {
            return controlActive;
        }

        /// <summary>
        /// Button state updated
        /// </summary>
        /// <param name="state"></param>
        protected virtual void OnButtonStateUpdated(UiButton.STATE state) { }

        /// <summary>
        /// Controll
        /// </summary>
        private void Update() {

            // Active?
            if (!IsControlActive()) {
                return;
            }

            // Interractable ?
            if (!Button.IsInteractable()) {
                return;
            }

            UpdateControl();

        }

        /// <summary>
        /// Update the control
        /// </summary>
        protected abstract void UpdateControl();

    }

}