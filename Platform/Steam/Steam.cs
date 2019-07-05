#if STEAM_BUILD

using System;
using System.Collections.Generic;
using Facepunch.Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

namespace BulwarkStudios.GameSystems.Platform {

    public sealed class Steam : BasePlatform<ISteamConfig> {

        /// <summary>
        /// Steam client
        /// </summary>
        private Client client;

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize() {

			Config.ForUnity(Application.platform.ToString());

			Debug.Log("Initialize Steam");

            // Restart if not steam ?
            if (Client.RestartIfNecessary(config.GetAppId())) {
                Debug.Log("Restart Steam");
                Application.Quit();
                return;
            }

            // Initialize the Api         
            client = new Client(config.GetAppId());

            Debug.Log("Steam OK: " + client.SteamId + " - " + client.Username);

        }

        /// <summary>
        /// Get the current language
        /// </summary>
        /// <returns></returns>
        protected override SystemLanguage GetLanguage() {
            if (client == null) {
                return Application.systemLanguage;
            }

            switch (client.CurrentLanguage) {
                case "english":
                    return SystemLanguage.English;
                case "german":
                    return SystemLanguage.German;
                case "french":
                    return SystemLanguage.French;
                case "italian":
                    return SystemLanguage.Italian;
                case "spanish":
                    return SystemLanguage.Spanish;
                case "russian":
                    return SystemLanguage.Russian;
                case "polish":
                    return SystemLanguage.Polish;
                case "koreana":
                    return SystemLanguage.Korean;
                case "japanese":
                    return SystemLanguage.Japanese;
                case "schinese":
                    return SystemLanguage.ChineseSimplified;
                case "tchinese":
                    return SystemLanguage.ChineseTraditional;
                case "turkish":
                    return SystemLanguage.Turkish;
                default:
                    return SystemLanguage.English;
            }

        }

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns></returns>
        protected override string GetUserName() {
            if (client == null) {
                return string.Empty;
            }

            return client.Username;

        }

        /// <summary>
        /// Update
        /// </summary>
        protected override void Update() {
            if (client != null) {
                client.Update();
            }
        }

        /// <summary>
        /// Destroy
        /// </summary>
        protected override void OnDestroy() {
            if (client != null) {
                client.Dispose();
            }
        }

        /// <summary>
        /// Has a dlc ?
        /// </summary>
        protected override bool HasDlc(PlatformDlc dlc) {

            if (client == null || client.App == null) {
                return false;
            }

            Debug.Log("HasDlc purchase: " + client.App.PurchaseTime(dlc.steamAppDlcId) + " " + DateTime.MinValue);

            return client.App.IsDlcInstalled(dlc.steamAppDlcId);

        }

        /// <summary>
        /// Reset all achievements
        /// </summary>
        protected override void ResetAllAchievements() {

            if (client == null || client.Achievements == null) {
                return;
            }

            foreach (Achievement achievement in client.Achievements.All) {
                client.Achievements.Reset(achievement.Id);
            }

        }

        /// <summary>
        /// Reset an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected override void ResetAchievement(PlatformAchievement achievement) {

            if (client == null || client.Achievements == null) {
                return;
            }

            client.Achievements.Reset(achievement.steamId);

        }

        /// <summary>
        /// Unlock an achievment
        /// </summary>
        /// <param name="achievement"></param>
        protected override void UnlockAchievement(PlatformAchievement achievement) {

            if (client == null || client.Achievements == null) {
                return;
            }

            // Constraint?
            if (achievementUnlockConstraint != null && achievementUnlockConstraint()) {
                Debug.Log("UnlockAchievement constrained: " + achievement.steamId + " " + achievement.description);
                return;
            }

            Debug.Log("Achievement unlocked: " + achievement.steamId + " " + achievement.description);

            client.Achievements.Trigger(achievement.steamId);

        }

        /// <summary>
        /// Refresh an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected override void RefreshAchievement(PlatformAchievement achievement) { }

        /// <summary>
        /// Analytics enabled state
        /// </summary>
        /// <param name="value"></param>
        protected override void AnalyticsEnabled(bool value) {
            Analytics.enabled = value;
            PerformanceReporting.enabled = value;
        }

        /// <summary>
        /// Analytics game start
        /// </summary>
        protected override void AnalyticsGameStart() {
            AnalyticsEvent.GameStart();
        }

        /// <summary>
        /// Analytics game over
        /// </summary>
        /// <param name="name"></param>
        protected override void AnalyticsGameOver(string name) {
            AnalyticsEvent.GameOver(name);
        }

        /// <summary>
        /// Analytics level start
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelStart(string name, IDictionary<string, object> eventData = null) {
            AnalyticsEvent.LevelStart(name, eventData);
        }

        /// <summary>
        /// Analytics level up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelUp(string name, IDictionary<string, object> eventData = null) {
            AnalyticsEvent.LevelUp(name, eventData);
        }

        /// <summary>
        /// Analytics level complete
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelComplete(string name, IDictionary<string, object> eventData = null) {
            AnalyticsEvent.LevelComplete(name, eventData);
        }

        /// <summary>
        /// Analytics level fail
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected override void AnalyticsLevelFail(string name, IDictionary<string, object> eventData = null) {
            AnalyticsEvent.LevelFail(name, eventData);
        }

        /// <summary>
        /// Analytics cutscene skip
        /// </summary>
        /// <param name="name"></param>
        protected override void AnalyticsCutsceneSkip(string name) {
            AnalyticsEvent.CutsceneSkip(name);
        }

        /// <summary>
        /// Analytics tutorial skip
        /// </summary>
        /// <param name="name"></param>
        protected override void AnalyticsTutorialSkip(string name) {
            AnalyticsEvent.TutorialSkip(name);
        }

    }

}

#endif