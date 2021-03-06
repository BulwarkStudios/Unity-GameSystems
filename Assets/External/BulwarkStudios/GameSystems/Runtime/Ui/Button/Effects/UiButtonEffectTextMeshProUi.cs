﻿using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

namespace BulwarkStudios.GameSystems.Ui {

    public class UiButtonEffectTextMeshProUi : UiButtonEffect<UiButtonEffectDataTextMeshProUi> {

        /// <summary>
        /// Ref to the text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI text = null;

        /// <summary>
        /// Update the text?
        /// </summary>
        [ToggleGroup(nameof(updateText), 0, "Update text", CollapseOthersOnExpand = false)]
        public bool updateText = false;

        /// <summary>
        /// New text
        /// </summary>
        [ToggleGroup(nameof(updateText), CollapseOthersOnExpand = false)]
        public string newText;

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

            // No ref?
            if (text == null) {
                return;
            }

            UiButtonEffectDataTextMeshProUi dataState = GetButtonUpdate(state);

            // Update the color
            if (updateColor) {
                text.color = dataState.color;
            }

            // Update the text
            if (updateText) {
                text.text = newText;
            }

        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="state"></param>
        protected override void End(UiButton.STATE state) { }

    }

    [System.Serializable, HideLabel]
    public class UiButtonEffectDataTextMeshProUi : UiButtonEffectData {

        /// <summary>
        /// New color
        /// </summary>
        [ShowIf(nameof(CheckColor), false)]
        public Color color = Color.white;

        private bool CheckColor() {
            return CheckCondition("updateColor");
        }

    }

}