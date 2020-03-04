using BulwarkStudios.GameSystems.Ui;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace Mechanicus.UI {

    public class UiButtonEventSample : UiButtonEvent<UiButtonEventDataSample> {

        /// <summary>
        /// Event triggered
        /// </summary>
        /// <param name="data"></param>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        protected override void EventTriggered(UiButtonEventDataSample data, UiButton button, UiButton.EVENT evt,
            BaseEventData eventData) { }

    }

    [System.Serializable, HideLabel]
    public class UiButtonEventDataSample : UiButtonEventData {

        public string evtName;

    }

}