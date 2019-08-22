using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Platform {

    public interface IPlatform {

        /// <summary>
        /// Initialize the platform
        /// </summary>
        /// <param name="config"></param>
        void Initialize(IPlatformConfig config);

        /// <summary>
        /// Update
        /// </summary>
        void Update();

        /// <summary>
        /// Destroy
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// Get the language
        /// </summary>
        /// <returns></returns>
        SystemLanguage GetLanguage();

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        /// Has a dlc ?
        /// </summary>
        /// <param name="dlc"></param>
        bool HasDlc(PlatformDlc dlc);

        /// <summary>
        /// Buy a Dlc
        /// </summary>
        /// <param name="dlc"></param>
        /// <param name="result"></param>
        void BuyDlc(PlatformDlc dlc, Action<PlatformDlcResult> result);

        /// <summary>
        /// Set an achievement constraint
        /// </summary>
        /// <param name="constraint"></param>
        void SetAchievementUnlockConstraint(Func<bool> constraint);

        /// <summary>
        /// Reset all achievement
        /// </summary>
        void ResetAllAchievements();

        /// <summary>
        /// Reset an achievement
        /// </summary>
        /// <param name="achievement"></param>
        void ResetAchievement(PlatformAchievement achievement);

        /// <summary>
        /// Unlock an achievment
        /// </summary>
        /// <param name="achievement"></param>
        void UnlockAchievement(PlatformAchievement achievement);

        /// <summary>
        /// Refresh an achievement
        /// </summary>
        /// <param name="achievement"></param>
        void RefreshAchievement(PlatformAchievement achievement);

        /// <summary>
        /// Analytics enabled state
        /// </summary>
        /// <param name="value"></param>
        void AnalyticsEnabled(bool value);

        /// <summary>
        /// Analytics game start
        /// </summary>
        void AnalyticsGameStart();

        /// <summary>
        /// Analytics game over
        /// </summary>
        /// <param name="name"></param>
        void AnalyticsGameOver(string name);

        /// <summary>
        /// Analytics level start
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void AnalyticsLevelStart(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics level up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void AnalyticsLevelUp(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics level complete
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void AnalyticsLevelComplete(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics level fail
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void AnalyticsLevelFail(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics cutscene skip
        /// </summary>
        /// <param name="name"></param>
        void AnalyticsCutsceneSkip(string name);

        /// <summary>
        /// Analytics tutorial skip
        /// </summary>
        /// <param name="name"></param>
        void AnalyticsTutorialSkip(string name);

    }

}