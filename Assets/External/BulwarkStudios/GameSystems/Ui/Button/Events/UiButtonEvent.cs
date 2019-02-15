using Sirenix.OdinInspector;
using UnityEngine;

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

        /// <summary>
        /// Current state
        /// </summary>
        [ShowInInspector, System.NonSerialized, HideLabel, EnumToggleButtons]
        public UiButton.EVENT previewEvent = UiButton.EVENT.ON_DESELECT;

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DESELECT), Title("On Deselect")]
        private T onDeselect = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DESELECT), Title("On Deselect Disable")]
        private T onDeselectDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_MOVE), Title("On Move")]
        private T onMove = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_MOVE), Title("On Move")]
        private T onMoveDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_CLICK), Title("On Pointer Click")]
        private T onPointerClick = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_CLICK), Title("On Pointer Click Disable")]
        private T onPointerClickDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_DOWN), Title("On Pointer Down")]
        private T onPointerDown = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_DOWN), Title("On Pointer Down Disable")]
        private T onPointerDownDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_ENTER), Title("On Pointer Enter")]
        private T onPointerEnter = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_ENTER), Title("On Pointer Enter Disable")]
        private T onPointerEnterDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_EXIT), Title("On Pointer Exit")]
        private T onPointerExit = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_EXIT), Title("On Pointer Exit Disable")]
        private T onPointerExitDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_UP), Title("On Pointer Up")]
        private T onPointerUp = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_POINTER_UP), Title("On Pointer Up Disable")]
        private T onPointerUpDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SELECT), Title("On Select")]
        private T onSelect = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SELECT), Title("On Select Disable")]
        private T onSelectDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SUBMIT), Title("On Submit")]
        private T onSubmit = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SUBMIT), Title("On Subit Disable")]
        private T onSubmitDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_INITIALIZE_POTENTIAL_DRAG), Title("On Submit")]
        private T onInitializePotentialDrag = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_INITIALIZE_POTENTIAL_DRAG), Title("On Subit Disable")]
        private T onInitializePotentialDragDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_BEGIN_DRAG), Title("On Submit")]
        private T onBeginDrag = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_BEGIN_DRAG), Title("On Subit Disable")]
        private T onBeginDragDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DRAG), Title("On Submit")]
        private T onDrag = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DRAG), Title("On Subit Disable")]
        private T onDragDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_END_DRAG), Title("On Submit")]
        private T onEndDrag = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_END_DRAG), Title("On Subit Disable")]
        private T onEndDragDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DROP), Title("On Submit")]
        private T onDrop = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_DROP), Title("On Subit Disable")]
        private T onDropDisable = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SCROLL), Title("On Submit")]
        private T onScroll = new T();

        [PropertyOrder(100), SerializeField, ShowIf(nameof(GetButtonPreviewEvent), UiButton.EVENT.ON_SCROLL), Title("On Subit Disable")]
        private T onScrollDisable = new T();

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
        private void OnEventTriggered(UiButton button, UiButton.EVENT evt) {
            T data = GetData(button, evt);
            if (data == null) {
                return;
            }
            EventTriggered(data, button, evt);
        }

        /// <summary>
        /// Get the button state
        /// </summary>
        private UiButton.EVENT GetButtonPreviewEvent() {
            return previewEvent;
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
                    case UiButton.EVENT.ON_INITIALIZE_POTENTIAL_DRAG:
                        return onInitializePotentialDrag;
                    case UiButton.EVENT.ON_BEGIN_DRAG:
                        return onBeginDrag;
                    case UiButton.EVENT.ON_DRAG:
                        return onDrag;
                    case UiButton.EVENT.ON_END_DRAG:
                        return onEndDrag;
                    case UiButton.EVENT.ON_DROP:
                        return onDrop;
                    case UiButton.EVENT.ON_SCROLL:
                        return onScroll;
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
                case UiButton.EVENT.ON_INITIALIZE_POTENTIAL_DRAG:
                    return onInitializePotentialDragDisable;
                case UiButton.EVENT.ON_BEGIN_DRAG:
                    return onBeginDragDisable;
                case UiButton.EVENT.ON_DRAG:
                    return onDragDisable;
                case UiButton.EVENT.ON_END_DRAG:
                    return onEndDragDisable;
                case UiButton.EVENT.ON_DROP:
                    return onDropDisable;
                case UiButton.EVENT.ON_SCROLL:
                    return onScrollDisable;
            }

            return onDeselectDisable;

        }

        /// <summary>
        /// Event triggered
        /// </summary>
        /// <param name="data"></param>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        protected abstract void EventTriggered(T data, UiButton button, UiButton.EVENT evt);

    }

}
