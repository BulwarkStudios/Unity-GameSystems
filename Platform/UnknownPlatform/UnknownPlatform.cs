using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Platform {

    public class UnknownPlatformConfig : IUnknownPlatformConfig {

    }

    public sealed class UnknownPlatform : BasePlatform<IUnknownPlatformConfig> {

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize() { }

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns></returns>
        protected override string GetUserName() {
            return string.Empty;
        }

        /// <summary>
        /// Update
        /// </summary>
        protected override void Update() { }

        /// <summary>
        /// Destroy
        /// </summary>
        protected override void OnDestroy() { }

        /// <summary>
        /// Get the current language
        /// </summary>
        /// <returns></returns>
        protected override SystemLanguage GetLanguage() {
            return Application.systemLanguage;
        }

        /// <summary>
        /// Has a dlc ?
        /// </summary>
        protected override bool HasDlc(PlatformDlc dlc) {
            return false;
        }

        /// <summary>
        /// Buy a Dlc
        /// </summary>
        /// <param name="dlc"></param>
        /// <param name="result"></param>
        protected override void BuyDlc(PlatformDlc dlc, Action<PlatformDlcResult> result) {

            if (result != null) {
                result(new PlatformDlcResult(PlatformDlcResult.STATE.FAIL, "Not implemented"));
            }

        }

        /// <summary>
        /// Reset all achievements
        /// </summary>
        protected override void ResetAllAchievements() { }

        /// <summary>
        /// Reset an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected override void ResetAchievement(PlatformAchievement achievement) { }

        /// <summary>
        /// Unlock an achievment
        /// </summary>
        /// <param name="achievement"></param>
        protected override void UnlockAchievement(PlatformAchievement achievement) { }

        /// <summary>
        /// Refresh an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected override void RefreshAchievement(PlatformAchievement achievement) { }

        /// <summary>
        /// Analytics enabled state
        /// </summary>
        /// <param name="value"></param>
        protected override void AnalyticsEnabled(bool value) { }

        /// <summary>
        /// Analytics game start
        /// </summary>
        protected override void AnalyticsGameStart() { }

        /// <summary>
        /// Analytics game over
        /// </summary>
        /// <param name="name"></param>
        protected override void AnalyticsGameOver(string name) { }

        /// <summary>
        /// Analytics level start
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelStart(string name, IDictionary<string, object> eventData = null) { }

        /// <summary>
        /// Analytics level up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelUp(string name, IDictionary<string, object> eventData = null) { }

        /// <summary>
        /// Analytics level complete
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelComplete(string name, IDictionary<string, object> eventData = null) { }

        /// <summary>
        /// Analytics level fail
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelFail(string name, IDictionary<string, object> eventData = null) { }

        /// <summary>
        /// Analytics cutscene skip
        /// </summary>
        /// <param name="name"></param>
        protected override void AnalyticsCutsceneSkip(string name) { }

        /// <summary>
        /// Analytics tutorial skip
        /// </summary>
        /// <param name="name"></param>
        protected override void AnalyticsTutorialSkip(string name) { }

    }

}