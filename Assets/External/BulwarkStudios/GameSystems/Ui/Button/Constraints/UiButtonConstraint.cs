using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    [RequireComponent(typeof(UiButton))]
    public abstract class UiButtonConstraint : MonoBehaviour {

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
        /// Refresh the button state
        /// </summary>
        protected void RefreshButtonState() {
            Button.RefreshButtonState();
        }

        /// <summary>
        /// The contraint is valid?
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

    }

}
