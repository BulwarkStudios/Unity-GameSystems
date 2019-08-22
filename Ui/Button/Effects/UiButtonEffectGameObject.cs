using Sirenix.OdinInspector;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    public class UiButtonEffectGameObject : UiButtonEffect<UiButtonEffectDataGameObject> {

        /// <summary>
        /// Ref to the gameObject
        /// </summary>
        [SerializeField]
        private GameObject gObject = null;

        /// <summary>
        /// Update the active state?
        /// </summary>
        [SerializeField]
        private bool updateActive = false;

        /// <summary>
        /// Preview
        /// </summary>
        /// <param name="state"></param>
        protected override void Preview(UiButton.STATE state) {
            Begin(state);
        }

        /// <summary>
        /// Begin
        /// </summary>
        /// <param name="state"></param>
        protected override void Begin(UiButton.STATE state) {

            // No ref?
            if (gObject == null) {
                return;
            }

            UiButtonEffectDataGameObject dataState = GetButtonUpdate(state);

            // Update the game object
            if (updateActive) {
                gObject.SetActive(dataState.active);
            }

        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="state"></param>
        protected override void End(UiButton.STATE state) { }

    }

    [System.Serializable, HideLabel]
    public class UiButtonEffectDataGameObject : UiButtonEffectData {

        /// <summary>
        /// New active state
        /// </summary>
        [ShowIf(nameof(CheckActive), false)]
        public bool active = true;

        private bool CheckActive() {
            return CheckCondition("updateActive");
        }

    }

}