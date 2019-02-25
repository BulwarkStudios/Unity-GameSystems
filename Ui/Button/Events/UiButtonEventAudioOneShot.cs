using BulwarkStudios.GameSystems.Logs;
using BulwarkStudios.GameSystems.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanicus.UI {

    public class UiButtonEventAudioOneShot : UiButtonEvent<UiButtonEventDataAudioOneShot> {

        [SerializeField] private AudioSource audioSource = null;

        /// <summary>
        /// Event triggered
        /// </summary>
        /// <param name="data"></param>
        /// <param name="button"></param>
        /// <param name="evt"></param>
        protected override void EventTriggered(UiButtonEventDataAudioOneShot data, UiButton button, UiButton.EVENT evt, BaseEventData eventData) {

            if (audioSource == null || data.audioClip == null) {
                return;
            }

            audioSource.PlayOneShot(data.audioClip);

        }

    }

    [System.Serializable, HideLabel]
    public class UiButtonEventDataAudioOneShot : UiButtonEventData {

        public AudioClip audioClip;

    }

}
