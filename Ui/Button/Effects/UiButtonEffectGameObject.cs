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
            if (dataState.updateActive) {
                gObject.SetActive(dataState.active);
            }

        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="state"></param>
        protected override void End(UiButton.STATE state) {

        }

    }

    [System.Serializable, HideLabel]
    public class UiButtonEffectDataGameObject : UiButtonEffectData {

        /// <summary>
        /// Update the active state?
        /// </summary>
        [ToggleGroup(nameof(updateActive), 0, "Update active", CollapseOthersOnExpand = false)]
        public bool updateActive = false;

        /// <summary>
        /// New active state
        /// </summary>
        [ToggleGroup(nameof(updateActive), CollapseOthersOnExpand = false)]
        public bool active = true;

    }

}
