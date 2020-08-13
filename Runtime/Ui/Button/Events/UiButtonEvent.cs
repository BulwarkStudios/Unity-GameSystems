using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BulwarkStudios.GameSystems.Ui {

    [RequireComponent(typeof(UiButton))]
    public abstract class UiButtonEvent<T> : MonoBehaviour where T : UiButtonEventData, new() {

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

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DESELECT, false),
         Title("On Deselect")]
        private T onDeselect = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DESELECT, false),
         Title("On Deselect Disable")]
        private T onDeselectDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_MOVE, false),
         Title("On Move")]
        private T onMove = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_MOVE, false),
         Title("On Move")]
        private T onMoveDisable = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_CLICK, false), Title("On Pointer Click")]
        private T onPointerClick = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_CLICK, false),
         Title("On Pointer Click Disable")]
        private T onPointerClickDisable = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_DOWN, false), Title("On Pointer Down")]
        private T onPointerDown = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_DOWN, false), Title("On Pointer Down Disable")]
        private T onPointerDownDisable = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_ENTER, false), Title("On Pointer Enter")]
        private T onPointerEnter = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_ENTER, false),
         Title("On Pointer Enter Disable")]
        private T onPointerEnterDisable = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_EXIT, false), Title("On Pointer Exit")]
        private T onPointerExit = new T();

        [PropertyOrder(100), SerializeField,
         ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_EXIT, false), Title("On Pointer Exit Disable")]
        private T onPointerExitDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_UP, false),
         Title("On Pointer Up")]
        private T onPointerUp = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_UP, false),
         Title("On Pointer Up Disable")]
        private T onPointerUpDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SELECT, false),
         Title("On Select")]
        private T onSelect = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SELECT, false),
         Title("On Select Disable")]
        private T onSelectDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SUBMIT, false),
         Title("On Submit")]
        private T onSubmit = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SUBMIT, false),
         Title("On Submit Disable")]
        private T onSubmitDisable = new T();

        /// <summary>
        /// Enable
        /// </summary>
        private void OnEnable() {
            Button.OnEventTriggered += OnEventTriggered;
        }

        /// <summary>
        /// Disable
        /// </summary>
        private void OnDisable() {
            Button.OnEventTriggered -= OnEventTriggered;
        }

        /// <summary>
        /// Button event triggered
        /// </summary>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        /// <param name="eventData"></param>
        private void OnEventTriggered(UiButton button, UiButton.EVENT evt, BaseEventData eventData) {
            T data = GetData(button, evt);

            if (data == null) {
                return;
            }

            EventTriggered(data, button, evt, eventData);
        }

        /// <summary>
        /// Get the button state
        /// </summary>
        private UiButton.EVENT GetButtonPreviewEvent() {

            UiButton uiButton = GetComponent<UiButton>();

            if (uiButton == null) {
                return UiButton.EVENT.ON_DESELECT;
            }

            return uiButton.previewEvent;

        }

        /// <summary>
        /// Get the data depending on the event
        /// </summary>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        private T GetData(UiButton button, UiButton.EVENT evt) {

            // Interractable state
            if (button.IsInteractable()) {
                switch (evt) {
                    case UiButton.EVENT.ON_DESELECT:
                        return onDeselect;

                    case UiButton.EVENT.ON_MOVE:
                        return onMove;

                    case UiButton.EVENT.ON_POINTER_CLICK:
                        return onPointerClick;

                    case UiButton.EVENT.ON_POINTER_DOWN:
                        return onPointerDown;

                    case UiButton.EVENT.ON_POINTER_ENTER:
                        return onPointerEnter;

                    case UiButton.EVENT.ON_POINTER_EXIT:
                        return onPointerExit;

                    case UiButton.EVENT.ON_POINTER_UP:
                        return onPointerUp;

                    case UiButton.EVENT.ON_SELECT:
                        return onSelect;

                    case UiButton.EVENT.ON_SUBMIT:
                        return onSubmit;

                }

                return onDeselect;
            }

            // Not interractable
            switch (evt) {
                case UiButton.EVENT.ON_DESELECT:
                    return onDeselectDisable;

                case UiButton.EVENT.ON_MOVE:
                    return onMoveDisable;

                case UiButton.EVENT.ON_POINTER_CLICK:
                    return onPointerClickDisable;

                case UiButton.EVENT.ON_POINTER_DOWN:
                    return onPointerDownDisable;

                case UiButton.EVENT.ON_POINTER_ENTER:
                    return onPointerEnterDisable;

                case UiButton.EVENT.ON_POINTER_EXIT:
                    return onPointerExitDisable;

                case UiButton.EVENT.ON_POINTER_UP:
                    return onPointerUpDisable;

                case UiButton.EVENT.ON_SELECT:
                    return onSelectDisable;

                case UiButton.EVENT.ON_SUBMIT:
                    return onSubmitDisable;

            }

            return onDeselectDisable;

        }

        /// <summary>
        /// Event triggered
        /// </summary>
        /// <param name="data"></param>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        /// <param name="eventData"></param>
        protected abstract void EventTriggered(T data, UiButton button, UiButton.EVENT evt, BaseEventData eventData);

    }

}