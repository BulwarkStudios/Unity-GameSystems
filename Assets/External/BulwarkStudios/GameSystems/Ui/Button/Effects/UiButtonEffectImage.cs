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
        /// Update the sprite?
        /// </summary>
        [SerializeField]
        private bool updateSprite = false;

        /// <summary>
        /// Update the color?
        /// </summary>
        [SerializeField]
        private bool updateColor = false;

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
            if (updateSprite) {
                image.sprite = dataState.sprite;
            }

            // Update the color
            if (updateColor) {
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
        /// New sprite
        /// </summary>
        [ShowIf(nameof(CheckSprite), false)]
        public Sprite sprite = null;

        /// <summary>
        /// New color
        /// </summary>
        [ShowIf(nameof(CheckColor), false)]
        public Color color = Color.white;

        private bool CheckSprite() {
            return CheckCondition("updateSprite");
        }

        private bool CheckColor() {
            return CheckCondition("updateColor");
        }

    }

}
