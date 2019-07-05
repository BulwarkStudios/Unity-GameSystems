#if ORIGIN_BUILD

using System.Collections.Generic;
using Origin;
using Origin.Data;
using UnityEngine;
using UnityEngine.Analytics;

namespace BulwarkStudios.GameSystems.Platform {

    public sealed class Origin : BasePlatform<IOriginConfig> {

        /// <summary>
        /// Origin client
        /// </summary>
        private SDK client;

        /// <summary>
        /// Entitlements
        /// </summary>
        private List<EntitlementT> entitlements;

        /// <summary>
        /// Language
        /// </summary>
        private string language;

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize() {

            Debug.Log("Initialize Origin");

            client = new SDK();

            // Events
            //client.Disconnected += ClientDisconnected;

            // Modify the data below to match your game configuration
            SDK.StartupInputT input = new SDK.StartupInputT();
            input.ContentId = config.GetAppId();
            input.MultiplayerId = "";
            input.Language = "en_US";
            input.Title = "Warhammer 40k: Mechanicus";

            SDK.StartupOutputT output;

            // Start
            OriginErrorT err = client.Startup(SDK.OriginSDKFlags.None, 0, config.GetSecurityKey(), input, out output);

            // Error?
            if (err != OriginErrorT.ORIGIN_SUCCESS) {
                Debug.Log("Origin Error: " + err);
                client = null;
                return;
            }

            Debug.Log("Origin Loaded: " + output.ProductId + " " + output.ContentId + " " + output.Version + " User: " + client.DefaultUser + " Persona: " + client.DefaultPersona);

            // Load game info
            client.GetAllGameInfo(15000, (GetAllGameInfoResponseT response, OriginErrorT errGa) => {

                if (errGa != OriginErrorT.ORIGIN_SUCCESS) {
                    Debug.Log("Origin Get All game info failed: " + errGa);
                    return;
                }

                Debug.Log("Origin Game info: " + response.DisplayName + " Installed language: " + response.InstalledLanguage);

                language = response.InstalledLanguage;

            });

            // Get entitlements
            client.QueryEntitlements(client.DefaultUser, "OPP", true, null, null, null, null, 15000,
                (QueryEntitlementsResponseT response, OriginErrorT errEn) => {

                    if (errEn != OriginErrorT.ORIGIN_SUCCESS) {
                        Debug.Log("Origin Query Entitlements failed: " + errEn);
                        return;
                    }

                    entitlements = response.Entitlements;

                    Debug.Log("Origin Entitlement loaded " + entitlements);

                    foreach (EntitlementT entitlement in entitlements) {
                        Debug.Log("Origin Entitlement: " + entitlement.Type + " " + entitlement.ItemId + " " + entitlement.EntitlementId + " " + entitlement.EntitlementTag);
                    }

                });

        }

        /// <summary>
        /// Get the current language
        /// </summary>
        /// <returns></returns>
        protected override SystemLanguage GetLanguage() {

            if (string.IsNullOrEmpty(language)) {
                return Application.systemLanguage;
            }

            switch (language) {
                case "en_GB":
                    return SystemLanguage.English;
                case "en_US":
                    return SystemLanguage.English;
                case "de_DE":
                    return SystemLanguage.German;
                case "fr_FR":
                    return SystemLanguage.French;
                case "it_IT":
                    return SystemLanguage.Italian;
                case "es_ES":
                    return SystemLanguage.Spanish;
                case "ru_RU":
                    return SystemLanguage.Russian;
                case "pl_PL":
                    return SystemLanguage.Polish;
                case "ko_KR":
                    return SystemLanguage.Korean;
                case "ja_JP":
                    return SystemLanguage.Japanese;
                case "zh_CN":
                    return SystemLanguage.ChineseSimplified;
                case "zh_TW":
                    return SystemLanguage.ChineseTraditional;
                default:
                    return SystemLanguage.English;
            }

        }

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns></returns>
        protected override string GetUserName() {

            if (client != null) {
                return client.DefaultUser.ToString();
            }

            return string.Empty;

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
                client.Shutdown();
                client = null;
            }
        }

        /// <summary>
        /// Has a dlc ?
        /// </summary>
        protected override bool HasDlc(PlatformDlc dlc) {

            if (client == null || entitlements == null) {
                return false;
            }

            foreach (EntitlementT entitlement in entitlements) {
                if (entitlement.EntitlementTag.Equals(dlc.originEntitlementTag)) {
                    Debug.Log("Origin Has Dlc: " + dlc.originEntitlementTag);
                    return true;
                }
            }

            Debug.Log("Origin Has Not Dlc: " + dlc.originEntitlementTag);

            return false;
        }

        /// <summary>
        /// Reset all achievements
        /// </summary>
        protected override void ResetAllAchievements() {
            
        }

        /// <summary>
        /// Reset an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected override void ResetAchievement(PlatformAchievement achievement) {
            
        }

        /// <summary>
        /// Unlock an achievment
        /// </summary>
        /// <param name="achievement"></param>
        protected override void UnlockAchievement(PlatformAchievement achievement) {

            if (client == null) {
                return;
            }

            Debug.Log("Origin Try UnlockAchievement: " + achievement.originId + " " + achievement.description);

            // Constraint?
            if (achievementUnlockConstraint != null && achievementUnlockConstraint()) {
                Debug.Log("Origin UnlockAchievement constrained: " + achievement.originId + " " + achievement.description);
                return;
            }

            Debug.Log("Origin Try UnlockAchievement: " + achievement.originId + " " + achievement.description + " DefaultUser: " + client.DefaultUser + " Persona: " + client.DefaultPersona + " Code: " + config.GetAchievementCode());

            // Try to unlock
            client.GrantAchievement(client.DefaultUser, client.DefaultPersona, achievement.originId,
                config.GetAchievementCode(), 1, 15000,
                (response, err) => {

                    if (err == OriginErrorT.ORIGIN_SUCCESS) {
                        Debug.Log("Origin Achievement unlocked: " + achievement.originId + " " + achievement.description);
                    }
                    else {
                        Debug.Log("Origin Achievement unlocked failed: " + achievement.originId + " " + achievement.description + " " + err);
                    }

                });

        }

        /// <summary>
        /// Refresh an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected override void RefreshAchievement(PlatformAchievement achievement) {

        }

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