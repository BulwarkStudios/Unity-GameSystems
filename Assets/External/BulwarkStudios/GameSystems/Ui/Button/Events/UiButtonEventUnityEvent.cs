using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace BulwarkStudios.GameSystems.Ui {

    public class UiButtonEventUnityEvent : UiButtonEvent<UiButtonEventDataUnityEvent> {

        /// <summary>
        /// Event triggered
        /// </summary>
        /// <param name="data"></param>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        /// <param name="eventData"></param>
        protected override void EventTriggered(UiButtonEventDataUnityEvent data, UiButton button, UiButton.EVENT evt, BaseEventData eventData) {

            data.unityEvent?.Invoke(eventData);

        }

    }

    [System.Serializable, HideLabel]
    public class UiButtonEventDataUnityEvent : UiButtonEventData {

        public EventTrigger.TriggerEvent unityEvent;

    }

}
