using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace BulwarkStudios.GameSystems.Ui {

    public class UiButtonEffectImage : UiButtonEffect<UiButtonEffectDataImage> {

        /// <summary>
        /// Ref to the image
        /// </summary>
        [SerializeField]
        private Image image = null;

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

            // No image?
            if (image == null) {
                return;
            }

            UiButtonEffectDataImage dataState = GetButtonUpdate(state);

            // Update the sprite
            if (dataState.updateSprite) {
                image.sprite = dataState.sprite;
            }

            // Update the color
            if (dataState.updateColor) {
                image.color = dataState.color;
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
    public class UiButtonEffectDataImage : UiButtonEffectData {

        /// <summary>
        /// Update the sprite?
        /// </summary>
        [ToggleGroup(nameof(updateSprite), 0, "Update sprite", CollapseOthersOnExpand = false)]
        public bool updateSprite = false;

        /// <summary>
        /// New sprite
        /// </summary>
        [ToggleGroup(nameof(updateSprite), CollapseOthersOnExpand = false)]
        public Sprite sprite = null;

        /// <summary>
        /// Update the color?
        /// </summary>
        [ToggleGroup(nameof(updateColor), 0, "Update color", CollapseOthersOnExpand = false)]
        public bool updateColor = false;

        /// <summary>
        /// New color
        /// </summary>
        [ToggleGroup(nameof(updateColor), CollapseOthersOnExpand = false)]
        public Color color = Color.white;

    }

}
