using Sirenix.OdinInspector;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Ui {

    public abstract class UiButtonEffect<T> : MonoBehaviour, IUiButtonEffect where T : UiButtonEffectData, new() {

        /// <summary>
        /// Normal state info
        /// </summary>
        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewState), UiButton.STATE.NORMAL, false),
         Title("Normal")]
        private T normal = new T();

        /// <summary>
        /// Highlighted state info
        /// </summary>
        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewState), UiButton.STATE.HIGHLIGHTED, false),
         Title("Highlighted")]
        private T highlighted = new T();

        /// <summary>
        /// Pressed state info
        /// </summary>
        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewState), UiButton.STATE.PRESSED, false),
         Title("Pressed")]
        private T pressed = new T();

        /// <summary>
        /// Disable state info
        /// </summary>
        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewState), UiButton.STATE.DISABLED, false),
         Title("Disabled")]
        private T disabled = new T();

        /// <summary>
        /// Custom1 state info
        /// </summary>
        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewState), UiButton.STATE.CUSTOM1, false),
         Title("Custom 1")]
        private T custom1 = new T();

        /// <summary>
        /// Custom2 state info
        /// </summary>
        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewState), UiButton.STATE.CUSTOM2, false),
         Title("Custom 2")]
        private T custom2 = new T();

        /// <summary>
        /// Get the button update property depending on a state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected T GetButtonUpdate(UiButton.STATE state) {

            T data = normal;

            switch (state) {
                case UiButton.STATE.NORMAL:
                    data = normal;
                    break;

                case UiButton.STATE.HIGHLIGHTED:
                    data = highlighted;
                    break;

                case UiButton.STATE.PRESSED:
                    data = pressed;
                    break;

                case UiButton.STATE.DISABLED:
                    data = disabled;
                    break;

                case UiButton.STATE.CUSTOM1:
                    data = custom1;
                    break;

                case UiButton.STATE.CUSTOM2:
                    data = custom2;
                    break;
            }

            data.SetButtonEffect(this);

            return data;
        }

        /// <summary>
        /// Get the button state
        /// </summary>
        private UiButton.STATE GetButtonPreviewState() {

            UiButton uiButton = GetComponent<UiButton>();

            if (uiButton == null) {
                return UiButton.STATE.NORMAL;
            }

            if (!Application.isPlaying) {
                ((IUiButtonEffect) this).PreviewState(uiButton.previewState);
            }

            return uiButton.previewState;
        }

        /// <summary>
        /// Preview
        /// </summary>
        /// <param name="state"></param>
        protected abstract void Preview(UiButton.STATE state);

        /// <summary>
        /// Begin
        /// </summary>
        /// <param name="state"></param>
        protected abstract void Begin(UiButton.STATE state);

        /// <summary>
        /// End
        /// </summary>
        /// <param name="state"></param>
        protected abstract void End(UiButton.STATE state);

        /// <summary>
        /// Preview the state
        /// </summary>
        /// <param name="newState"></param>
        void IUiButtonEffect.PreviewState(UiButton.STATE newState) {

            Preview(newState);

        }

        /// <summary>
        /// Update the state
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        void IUiButtonEffect.UpdateState(UiButton.STATE oldState, UiButton.STATE newState) {

            End(oldState);
            Begin(newState);

        }

    }

}